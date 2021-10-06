using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Globalization;

public class MaillageLoader : MonoBehaviour
{
    public Material mat;
    Vector3[] verticesList;
    int[] triangleList;
    Vector3 gravityCenter;
    int nb_vertices;
    int nb_aretes;
    int nb_faces;
    Vector3[] normalsList;

    //Vecterx clustering
    grid4Clustering[] grid;
    public float pas = 0.1f;
    List<int> simplifiedTriangles;

    // Start is called before the first frame update
    void Start()
    {
        Mesh msh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gridMaker();
        ImportOFF();
        
        transform.position = gravityCenter;

        ComputeNormals();
        //NormalizeMesh();

        getVerticesInCell();
        replaceTriangles();
        removeUselessTriangles();

        msh.vertices = verticesList;
        msh.triangles = simplifiedTriangles.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ImportOFF()
    {
        string[] lines = File.ReadAllLines(@"Assets/buddha.off");

        //Line 1 
        string[] line1Split = lines[1].Split(' ');

        //Get essential values
        int.TryParse(line1Split[0], out nb_vertices);
        int.TryParse(line1Split[1], out nb_faces);
        nb_aretes = nb_faces * 3;
       
        //Get vertices list
        verticesList = new Vector3[nb_vertices];

        float sumVerticesX = 0f;
        float sumVerticesY = 0f;
        float sumVerticesZ = 0f;

        float max = 0f;

        for (int i = 0; i < nb_vertices; i++)
        {
            string[] lineVerticesCoord = lines[2 + i].Split(' ');
            float x = float.Parse(lineVerticesCoord[0], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
            float y = float.Parse(lineVerticesCoord[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
            float z = float.Parse(lineVerticesCoord[2], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
            verticesList[i] = (new Vector3(x, y, z));
            sumVerticesX += x;
            sumVerticesY += y;
            sumVerticesZ += z;

            if(Mathf.Abs(x)>max)
            {
                max = Mathf.Abs(x);
            }
            if (Mathf.Abs(y) > max)
            {
                max = Mathf.Abs(y);
            }
            if (Mathf.Abs(z) > max)
            {
                max = Mathf.Abs(z);
            }
        }
           
        gravityCenter = new Vector3((sumVerticesX / nb_vertices), (sumVerticesX / nb_vertices), (sumVerticesX / nb_vertices));

        //Normalize mesh to the farthest point to have each point between 1/-1
        for (int k = 0; k < verticesList.Length; k++)
        {
            verticesList[k] = new Vector3((verticesList[k].x / max), (verticesList[k].y / max), (verticesList[k].z / max));
        }

        
        //Get the triangles list
        triangleList = new int[nb_aretes];
        int index = 0;

        for (int j = 0; j < nb_faces; j++)
        {
            string[] lineTriangle = lines[2 + nb_vertices + j].Split(' ');

            int vert1 = int.Parse(lineTriangle[1], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
            triangleList[index++] = vert1;
            int vert2 = int.Parse(lineTriangle[2], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
            triangleList[index++] = vert2;
            int vert3 = int.Parse(lineTriangle[3], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, CultureInfo.InvariantCulture);
            triangleList[index++] = vert3;
        }
    }

    

    void ComputeNormals()
    {
        normalsList = new Vector3[nb_aretes];
        int index=0;
        for(int i = 0; i< nb_faces; i+=3)
        {
            Vector3 u = verticesList[triangleList[i + 1]] - verticesList[triangleList[i]];
            Vector3 v = verticesList[triangleList[i + 2]] - verticesList[triangleList[i]];
            Vector3 normal = new Vector3(((u.y*v.z)-(u.z*v.y)), ((u.z * v.x) - (u.x * v.z)), ((u.x * v.y) - (u.y * v.z)));

            normalsList[index] = normal;
            index++;

        }
    }

    void gridMaker()
    {
        
        int index = 0;
        int gridSize = (int)Math.Pow(((2.2f+pas)/pas),3f);
        grid = new grid4Clustering[gridSize+1];

        for(float x = (gravityCenter.x-1.1f); x< (gravityCenter.x + 1.1f); x+=pas)
        {
            for (float y = (gravityCenter.y - 1.1f); y < (gravityCenter.y + 1.1f); y += pas)
            {
                for (float z = (gravityCenter.z - 1.1f); z < (gravityCenter.z + 1.1f); z += pas)
                {
                    grid[index].cellCenter = new Vector3(x, y, z);
                    grid[index].verticesInCell = new List<int>();
                    index++;
                }
            }
        }
    }

    public struct grid4Clustering
    {
        public Vector3 cellCenter;
        public List<int> verticesInCell; 
    }

    public void getVerticesInCell()
    {
        
        foreach(grid4Clustering cell in grid)
        {
            float xMin = cell.cellCenter.x - (pas/2);
            float xMax = cell.cellCenter.x + (pas / 2);
            float yMin = cell.cellCenter.y - (pas / 2);
            float yMax = cell.cellCenter.y + (pas / 2);
            float zMin = cell.cellCenter.z - (pas / 2);
            float zMax = cell.cellCenter.z + (pas / 2);


            for (int i = 0; i < nb_vertices; i++)
            {
                if (((verticesList[i].x <= xMax) & (verticesList[i].x >= xMin)) &
                    ((verticesList[i].y <= yMax) & (verticesList[i].y >= yMin)) &
                    ((verticesList[i].z <= zMax) & (verticesList[i].z >= zMin)) )
                {
                    cell.verticesInCell.Add(i);
                }
            }
        }
        
    }

    public void replaceTriangles()
    {
        for(int i = 0; i < nb_aretes; i++)
        {
            foreach(grid4Clustering cell in grid)
            {
                if(cell.verticesInCell != null)
                {
                    if (cell.verticesInCell.Contains(triangleList[i]))
                    {
                        triangleList[i] = cell.verticesInCell[0];
                    }
                }
            }
        }
    }

    public void removeUselessTriangles()
    {
        simplifiedTriangles = new List<int>();
        for (int i = 0; i < nb_aretes; i+=3)
        {
            if(  (triangleList[i] != triangleList[i+1]) & (triangleList[i] != triangleList[i + 2]) & (triangleList[i+2] != triangleList[i + 1]))
            {
                simplifiedTriangles.Add(triangleList[i]);
                simplifiedTriangles.Add(triangleList[i+1]);
                simplifiedTriangles.Add(triangleList[i+2]);
            }
        }
    }

}
