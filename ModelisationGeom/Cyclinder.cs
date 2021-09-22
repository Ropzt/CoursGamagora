using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclinder : MonoBehaviour
{
    public Material mat;
    
    
    // Start is called before the first frame update
    void Start()
    {
        float hauteur = 4.0f;

        Vector3 centre1 = new Vector3(0f,0f,hauteur);
        Vector3 centre2 = new Vector3(0f,0f,(hauteur/2));

        GameObject cercle1 = new GameObject();
        GameObject cercle2 = new GameObject();

        MakeCircle(cercle1,4.0f,1.0f,30, centre1);
        MakeCircle(cercle2,4.0f,1.0f,30, centre2);
        RelierCercles(cercle1, cercle2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeCircle(GameObject gameObject, float hauteur, float ray, int nombre_points, Vector3 vecCentre)
    {
        float h = hauteur;
        Vector3 center = vecCentre;
        int nb_points = nombre_points;
        float rayon = ray;

        Mesh msh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[nb_points+1];

        for( int i=0; i<nb_points; i++)
        {
            float angle = ((2*Mathf.PI*i)/nb_points);
            float x = rayon*Mathf.Cos(angle);
            float y = rayon*Mathf.Sin(angle);
            float z = center.z;
            vertices[i]=new Vector3(x,y,z);
        }
        vertices[nb_points] = center;
    
        List<int> triangle = new List<int>();

        for(int i=0;i<nb_points;i++)
        {
            triangle.Add(nb_points);
            triangle.Add(i);
            triangle.Add(i+1);
        }
        triangle.Add(nb_points);
        triangle.Add(nb_points-1);
        triangle.Add(0);

        msh.vertices = vertices;
        msh.triangles = triangle.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

    void RelierCercles(GameObject cercle1, GameObject cercle2)
    {
        Mesh meshC1 = cercle1.GetComponent<MeshFilter>().mesh;
        Mesh meshC2 = cercle2.GetComponent<MeshFilter>().mesh;

        Mesh msh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        Vector3[] vertices = new Vector3[(meshC1.vertices.Length-1)*2];

        for(int i = 0 ; i < meshC1.vertices.Length-1 ; i++ )
        {
            vertices[i] = meshC1.vertices[i];
            vertices[i+meshC1.vertices.Length-1] = meshC2.vertices[i];
        }

        List<int> triangles = new List<int>();

        for(int i = 0; i < ((vertices.Length/2)-1) ; i++)
        {
            //Premier triangle
            triangles.Add((vertices.Length/2)+i);
            Debug.Log("Triangle 1 de "+i+" Index P1= "+((vertices.Length/2)+i));
            triangles.Add((vertices.Length/2)+i+1);
            Debug.Log("Triangle 1 de "+i+" Index P2= "+((vertices.Length/2)+i+1));
            triangles.Add(i+1);
            Debug.Log("Triangle 1 de "+i+" Index P3= "+(i+1));
            

            //Second triangle
            triangles.Add((vertices.Length/2)+i);
            Debug.Log("Triangle 2 de "+i+" Index P1= "+((vertices.Length/2)+i));
            triangles.Add(i+1);
            Debug.Log("Triangle 2 de "+i+" Index P1= "+(i+1));
            triangles.Add(i);
            Debug.Log("Triangle 2 de "+i+" Index P1= "+i);
        }

        int j = ((vertices.Length/2)-1);
        //Premier triangle
        triangles.Add((vertices.Length/2)+j);
        Debug.Log("Triangle 1 de "+j+" Index P1= "+((vertices.Length/2)+j));
        triangles.Add((vertices.Length/2));
        Debug.Log("Triangle 1 de "+j+" Index P2= "+(vertices.Length/2));
        triangles.Add(0);
        Debug.Log("Triangle 1 de "+j+" Index P3= "+(j+1));

        //Second triangle
        triangles.Add((vertices.Length/2)+j);
        Debug.Log("Triangle 2 de "+j+" Index P1= "+((vertices.Length/2)+j));
        triangles.Add(0);
        Debug.Log("Triangle 1 de "+j+" Index P2= "+(j+1));
        triangles.Add(j);
        Debug.Log("Triangle 1 de "+j+" Index P3= "+j);

        msh.vertices = vertices;
        msh.triangles = triangles.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;
    }
}
