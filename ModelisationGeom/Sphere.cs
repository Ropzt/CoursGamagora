using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Material mat;
    public int nb_paralleles;
    public int nb_meridiens;
    public int hauteur;
    public float rayon;
 
    // Start is called before the first frame update
    void Start()
    {
        float ecartParalleles = hauteur/nb_paralleles;
        int nb_vertices = (nb_meridiens*nb_paralleles)+nb_paralleles;
        Vector3 centre = new Vector3(0f,0f,0f);

        Mesh msh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        //List<Vector3[]> listVert = new List<Vector3[]>();
        Vector3[] vertices = new Vector3[nb_vertices];

        int indexVertices = 0;

        for(int j=0; j<nb_paralleles; j++)
        {
            for( int i=0; i<nb_meridiens; i++)
            {
                float phi = ((Mathf.PI*j)/nb_paralleles);

                float angle = ((2*Mathf.PI*i)/nb_meridiens);

                float x = rayon*Mathf.Cos(angle)*Mathf.Sin(phi);
                float y = rayon*Mathf.Sin(angle)*Mathf.Sin(phi);
                float z = rayon*Mathf.Cos(phi);

                Debug.Log("X = "+x+" , Y = "+y+" , Z = "+z);

                vertices[indexVertices]=new Vector3(x,y,z);
                indexVertices++;
            }
        }
        Debug.Log("Index Vertice ="+indexVertices);

        for(int j=1; j<nb_paralleles; j++)
        {
            //Gestion des centres de chaque paralleles (les mettres à la fin)
            vertices[(nb_vertices-nb_paralleles)+j] = new Vector3(0f,0f, (0f-(hauteur/2)+(ecartParalleles*j)));
        }


        List<int> triangle = new List<int>();

        for(int j = 0; j < nb_paralleles ; j++)
        {
            for(int i=0;i<nb_meridiens;i++)
            {
                triangle.Add((nb_vertices-nb_paralleles)+j);
                triangle.Add(((j*nb_meridiens))+i);
                triangle.Add(((j*nb_meridiens))+i+1);

                Debug.Log("P1 = "+((nb_vertices-nb_paralleles)+j)+" , P2 = "+((j*nb_meridiens)+i)+" , Z = "+((j*nb_meridiens)+i+1));
            }
        }

        msh.vertices = vertices;
        msh.triangles = triangle.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;

    }

    
}
