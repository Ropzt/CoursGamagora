using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGridAlternate : MonoBehaviour
{
    [SerializeField] private float hauteur = 10f;
    [SerializeField] private float largeur = 10f;
    [SerializeField] private float profondeur = 10f;
    [SerializeField] private float cube_size = 1f;


    [SerializeField] private bool isIntersection = false;
    [SerializeField] private int nb_sphere = 2;
    [SerializeField] private float sphere_Radius = 1f;
    [SerializeField] private float posX_Sphere1 = 1f;
    [SerializeField] private float posY_Sphere1 = 1f;
    [SerializeField] private float posZ_Sphere1 = 1f;
    [SerializeField] private float posX_Sphere2 = 1f;
    [SerializeField] private float posY_Sphere2 = 1f;
    [SerializeField] private float posZ_Sphere2 = 1f;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> centreCubes = new List<Vector3>();
        for (float x =0f; x<hauteur; x+=cube_size)
        {
            for(float y = 0f; y < largeur; y += cube_size)
            {
                for(float z =0f; z<profondeur; z += cube_size)
                {
                    centreCubes.Add(new Vector3(x, y, z));
                }
            }
        }

        Vector3 centerSphere1 = new Vector3(posX_Sphere1, posY_Sphere1, posZ_Sphere1);
        Vector3 centerSphere2 = new Vector3(posX_Sphere2, posY_Sphere2, posZ_Sphere2);

        Vector3[] centreSpheres = new Vector3[nb_sphere];
        centreSpheres[0] = centerSphere1;
        centreSpheres[1] = centerSphere2;

        //We dont want to print a cube several times
        List<Vector3> computedCubes = new List<Vector3>();

        //Generate sphere
        foreach (Vector3 cubeCenter in centreCubes)
        {
            if (!computedCubes.Contains(cubeCenter))
            {
                
                for (int i = 0; i < nb_sphere; i++)
                {
                    float result = (Mathf.Pow((centreSpheres[i].x - cubeCenter.x), 2) + Mathf.Pow((centreSpheres[i].y - cubeCenter.y), 2) + Mathf.Pow((centreSpheres[i].z - cubeCenter.z), 2) - Mathf.Pow(sphere_Radius, 2));
                    if ( result< 0 )
                    {
                        if (isIntersection)
                        {
                            
                            for (int j = i + 1; j < nb_sphere; j++)
                            {
                                double resultIntersec = (Mathf.Pow((centreSpheres[j].x - cubeCenter.x), 2) + Mathf.Pow((centreSpheres[j].y - cubeCenter.y), 2) + Mathf.Pow((centreSpheres[j].z - cubeCenter.z), 2) - Mathf.Pow(sphere_Radius, 2));
                                
                                if (( resultIntersec< 0))
                                {
                                    computedCubes.Add(cubeCenter);
                                    GameObject cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    cubeObj.transform.parent = transform;
                                    cubeObj.transform.position = cubeCenter;
                                    cubeObj.transform.localScale = new Vector3(cube_size, cube_size,cube_size);
                                }
                            }
                        }
                        else if(result > -0.2f)
                        {
                            computedCubes.Add(cubeCenter);
                            GameObject cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cubeObj.transform.parent = transform;
                            cubeObj.transform.position = cubeCenter;
                            cubeObj.transform.localScale = new Vector3(cube_size, cube_size, cube_size);
                            
                        }
                    }
                }
            }
        }

    }

    
}
