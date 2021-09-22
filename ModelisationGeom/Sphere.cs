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
        float ecartParalleles = (float)hauteur/(float)nb_paralleles;
        int nb_vertices = (nb_meridiens*nb_paralleles)+nb_paralleles;
        Vector3 centre = new Vector3(0f,0f,0f);

        Mesh msh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        //List<Vector3[]> listVert = new List<Vector3[]>();
        Vector3[] vertices = new Vector3[nb_vertices];

        int indexVertices = 0;

        for(int j=1; j<nb_paralleles+1; j++)
        {
            for( int i=0; i<nb_meridiens; i++)
            {
                float phi = ((Mathf.PI*j)/nb_paralleles);

                float angle = ((2*Mathf.PI*i)/nb_meridiens);

                float x = rayon*Mathf.Cos(angle)*Mathf.Sin(phi);
                float y = rayon*Mathf.Sin(angle)*Mathf.Sin(phi);
                float z = rayon*Mathf.Cos(phi);

                //Debug.Log("Parallele "+j+", Point "+indexVertices+", X = "+x+" , Y = "+y+" , Z = "+z);

                vertices[indexVertices]=new Vector3(x,y,z);
                indexVertices++;
            }
        }

        //Ecriture des centres des cercles
        for(int j=1; j<nb_paralleles+1; j++)
        {
            float phi = ((Mathf.PI*j)/nb_paralleles);
            vertices[indexVertices+j-1] = new Vector3(0f,0f, (rayon*Mathf.Cos(phi)));
            
            //Debug.Log("Point "+(indexVertices+j-1)+", X = "+0+" , Y = "+0+" , Z = "+(rayon*Mathf.Cos(phi)));
        }

        //Ecriture des poles nord et sud
        // A FAIRE

        //Ecriture des triangles pour faire apparaitre les cercles
        List<int> triangle = new List<int>();

        for(int j = 0; j < nb_paralleles ; j++)
        {
            for(int i=0;i<nb_meridiens;i++)
            {
                triangle.Add((nb_vertices-nb_paralleles)+j);
                triangle.Add((j*nb_meridiens)+i);
                if((i+1) == nb_meridiens)
                {
                    triangle.Add((j*nb_meridiens));
                }
                else
                {
                    triangle.Add((j*nb_meridiens)+i+1);
                }

                //Debug.Log("P1 = "+((nb_vertices-nb_paralleles)+j)+" , P2 = "+((j*nb_meridiens)+i)+" , P3 = "+((j*nb_meridiens)+i+1));
            }
        }

        //Ecriture des triangles pour faire apparaitre la sphere
        List<int> outerTriangle = new List<int>();
        
        //Premier cercle vers pole 
        // A FAIRE

        //Cercles intermédiaires 
        for(int j = 1; j < nb_paralleles-1 ; j++) //j=1 car skip 1st circle; nb_paralleles-1 car skip last circle
        {
            for(int i=0;i<nb_meridiens;i++)
            {
                outerTriangle.Add((j*nb_meridiens)+i);
                outerTriangle.Add((j*nb_meridiens)+i+1);
                outerTriangle.Add(((j+1)*nb_meridiens)+i+1);

                outerTriangle.Add((j*nb_meridiens)+i);
                outerTriangle.Add(((j+1)*nb_meridiens)+i+1);
                outerTriangle.Add(((j+1)*nb_meridiens)+i);

                Debug.Log("P1 = "+((j*nb_meridiens)+i)+" , P2 = "+((j*nb_meridiens)+i+1)+" , P3 = "+(((j+1)*nb_meridiens)+i+1));
                Debug.Log("P1 = "+((j*nb_meridiens)+i)+" , P2 = "+(((j+1)*nb_meridiens)+i+1)+" , P3 = "+(((j+1)*nb_meridiens)+i));
            }
        }

        //dernier cercle vers pole
        //A FAIRE

        msh.vertices = vertices;
        //msh.triangles = triangle.ToArray();  //Si on veut faire apparaitre les cercles
        msh.triangles = outerTriangle.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;

    }

    
}
