using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Material mat;
    public int nb_paralleles;
    public int nb_meridiens;
    public float rayon;
    public bool isSphere = false;
 
    // Start is called before the first frame update
    void Start()
    {
        
        int nb_vertices = (nb_meridiens*nb_paralleles)+nb_paralleles;

        Mesh msh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        //List<Vector3[]> listVert = new List<Vector3[]>();
        Vector3[] vertices = new Vector3[nb_vertices+2];

        int indexVertices = 0;

        for(int j=0; j<nb_paralleles; j++)
        {
            float phi = ((Mathf.PI*(j+1))/(nb_paralleles+1));
            for( int i=0; i<nb_meridiens; i++)
            {
                float angle = ((2*Mathf.PI*i)/nb_meridiens);

                float x = rayon*Mathf.Cos(angle)*Mathf.Sin(phi);
                float y = rayon*Mathf.Sin(angle)*Mathf.Sin(phi);
                float z = rayon*Mathf.Cos(phi);

                Debug.Log("Parallele "+j+", Point "+indexVertices+", X = "+x+" , Y = "+y+" , Z = "+z);

                vertices[indexVertices]=new Vector3(x,y,z);
                indexVertices++;
            }
        }

        //Ecriture des centres des cercles
        for(int j=0; j<nb_paralleles+2; j++)
        {
            float phi = ((Mathf.PI*j)/(nb_paralleles+1));
            vertices[(nb_meridiens*nb_paralleles)+j] = new Vector3(0f,0f, (rayon*Mathf.Cos(phi)));
            
            Debug.Log("Point "+((nb_meridiens*nb_paralleles)+j)+", X = "+0+" , Y = "+0+" , Z = "+(rayon*Mathf.Cos(phi)));
        }

        /*
        //Ecriture des poles
        for(int j=0; j<2; j++)
        {
            float phi = ((Mathf.PI*(j+1))/(nb_paralleles+1));
            vertices[(nb_meridiens*nb_paralleles)+nb_paralleles+j] = new Vector3(0f,0f, (rayon*Mathf.Cos(phi)));
            
            Debug.Log("Point "+((nb_meridiens*nb_paralleles)+nb_paralleles+j)+", X = "+0+" , Y = "+0+" , Z = "+(rayon*Mathf.Cos(phi)));
        }
        */
 
        

        //Ecriture des triangles pour faire apparaitre les cercles internes
        /*List<int> triangle = new List<int>();

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
        }*/

        //Ecriture des triangles pour faire apparaitre la sphere
        List<int> outerTriangle = new List<int>();
        
        
        //Premier cercle vers pole 1
        for(int i =0; i<nb_meridiens;i++)
        {
            outerTriangle.Add(i);

            if((i+1) == nb_meridiens)
            {
                    outerTriangle.Add(0);
            }
            else
            {
                    outerTriangle.Add(i+1);
            }
            if(isSphere)
            {
                outerTriangle.Add((nb_paralleles*nb_meridiens));
            }
            else
            {
                outerTriangle.Add((nb_paralleles*nb_meridiens)+1);
            }
            
            Debug.Log("P1 = "+i+" , P2 = "+(i+1)+" , P3 = "+(nb_paralleles*nb_meridiens));
        }
        
        //Cercles intermédiaires 
        for(int j = 0; j < nb_paralleles-1 ; j++) //j=1 car skip 1st circle; nb_paralleles-1 car skip last circle
        {
            for(int i=0;i<nb_meridiens;i++)
            {
                if((i+1) == nb_meridiens)
                {
                    outerTriangle.Add((j*nb_meridiens)+i);
                    outerTriangle.Add((j*nb_meridiens));
                    outerTriangle.Add(((j+1)*nb_meridiens));

                    outerTriangle.Add((j*nb_meridiens)+i);
                    outerTriangle.Add(((j+1)*nb_meridiens));
                    outerTriangle.Add(((j+1)*nb_meridiens)+i);
                }
                else
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
        }

        
        //dernier cercle vers pole 2
        for(int i =0; i<nb_meridiens;i++)
        {
            outerTriangle.Add(((nb_paralleles-1)*nb_meridiens)+i);

            if((i+1) == nb_meridiens)
            {
                    outerTriangle.Add(((nb_paralleles-1)*nb_meridiens));
            }
            else
            {
                    outerTriangle.Add(((nb_paralleles-1)*nb_meridiens)+i+1);
            }
            if(isSphere)
            {
                outerTriangle.Add(nb_vertices+1);
            }
            else
            {
                outerTriangle.Add(nb_vertices);
            }
            
            Debug.Log("P1 = "+(((nb_paralleles-1)*nb_meridiens)+i)+" , P2 = "+(((nb_paralleles-1)*nb_meridiens)+i+1)+" , P3 = "+(nb_vertices+1));
        }
        

        msh.vertices = vertices;
        //msh.triangles = triangle.ToArray();  //Si on veut faire apparaitre les cercles internes
        msh.triangles = outerTriangle.ToArray();
        gameObject.GetComponent<MeshFilter>().mesh = msh;
        gameObject.GetComponent<MeshRenderer>().material = mat;

    }

    
}
