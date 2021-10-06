using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGrid : MonoBehaviour
{
    
    private int nb_recursion;
    [SerializeField] private float sphere_Radius = 1f;
    [SerializeField] private float cube_size = 1f;
    [SerializeField] private bool isIntersection = false;
    [SerializeField] private int nb_sphere = 2;
    [SerializeField] private float posX_Sphere1 = 1f;
    [SerializeField] private float posY_Sphere1 = 1f;
    [SerializeField] private float posZ_Sphere1 = 1f;
    [SerializeField] private float posX_Sphere2 = 1f;
    [SerializeField] private float posY_Sphere2 = 1f;
    [SerializeField] private float posZ_Sphere2 = 1f;

    // Start is called before the first frame update
    void Start()
    {

        Vector3 center = Vector3.zero;
        Vector3 centerSphere1 = new Vector3(posX_Sphere1, posY_Sphere1, posZ_Sphere1);
        Vector3 centerSphere2 = new Vector3(posX_Sphere2, posY_Sphere2, posZ_Sphere2);

        
        
        Vector3[] centreSpheres = new Vector3[nb_sphere];
        centreSpheres[0] = centerSphere1;
        centreSpheres[1] = centerSphere2;
        

        float minHeight = (Vector3.Distance(centerSphere1, centerSphere2) + sphere_Radius + sphere_Radius)*4f;

        

        int nb_recursion = 5;
        float nb_cubes = Mathf.Pow(2f,nb_recursion);
        float height = nb_cubes * cube_size;
        float pas = (cube_size * 2) / height;
        float newPas = 2f*pas;

        Debug.Log("cube_size = " + cube_size);
        Debug.Log("minHeight = " + minHeight);
        Debug.Log("height = " + height);
        Debug.Log("nb_recursion = " + nb_recursion);
        Debug.Log("pas = " + pas);

        List<Vector3> centreCubes = new List<Vector3>();
        List<Vector3> centreCubesTemp = new List<Vector3>();

        centreCubesTemp.Add(center);

        for (int i =0; i<nb_recursion;i++)
        {
            Debug.Log("Une récursion");
            //Initialize
            centreCubes = new List<Vector3>();
            newPas = newPas / 2;
            foreach(Vector3 centerTemp in centreCubesTemp)
            {
                //Diviser notre cube imaginaire initiale en 8 cube
                centreCubes.Add(new Vector3(centerTemp.x + newPas, centerTemp.y + newPas, centerTemp.z + newPas));//haut gauche front
                centreCubes.Add(new Vector3(centerTemp.x - newPas, centerTemp.y + newPas, centerTemp.z + newPas));//haut droit front
                centreCubes.Add(new Vector3(centerTemp.x + newPas, centerTemp.y - newPas, centerTemp.z + newPas));//bas gauche front
                centreCubes.Add(new Vector3(centerTemp.x - newPas, centerTemp.y - newPas, centerTemp.z + newPas));//bas droit front
                centreCubes.Add(new Vector3(centerTemp.x + newPas, centerTemp.y + newPas, centerTemp.z - newPas));//haut gauche back
                centreCubes.Add(new Vector3(centerTemp.x - newPas, centerTemp.y + newPas, centerTemp.z - newPas));//haut droit back
                centreCubes.Add(new Vector3(centerTemp.x + newPas, centerTemp.y - newPas, centerTemp.z - newPas));//bas gauche back
                centreCubes.Add(new Vector3(centerTemp.x - newPas, centerTemp.y - newPas, centerTemp.z - newPas));//bas droit back
            }
            //Reboot
            centreCubesTemp = centreCubes;
        }

        //We dont want to print a cube several times
        List<Vector3> computedCubes = new List<Vector3>();

        //Generate sphere
        foreach (Vector3 cubeCenter in centreCubes)
        {
            if(!computedCubes.Contains(cubeCenter))
            {
                
                for (int i = 0; i < nb_sphere; i++)
                {
                    float result = (Mathf.Pow((centreSpheres[i].x - cubeCenter.x), 2) + Mathf.Pow((centreSpheres[i].y - cubeCenter.y), 2) + Mathf.Pow((centreSpheres[i].z - cubeCenter.z), 2) - Mathf.Pow(sphere_Radius, 2));
                    if (result < 0)
                    {
                        if (isIntersection)
                        {
                            
                            for (int j = i + 1; j < nb_sphere; j++)
                            {
                                if ((Mathf.Pow((centreSpheres[j].x - cubeCenter.x), 2) + Mathf.Pow((centreSpheres[j].y - cubeCenter.y), 2) + Mathf.Pow((centreSpheres[j].z - cubeCenter.z), 2) - Mathf.Pow(sphere_Radius, 2)) < 0)
                                {
                                    computedCubes.Add(cubeCenter);
                                    GameObject cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                                    cubeObj.transform.parent = transform;
                                    cubeObj.transform.position = cubeCenter;
                                    cubeObj.transform.localScale = new Vector3((2f * newPas), (2f * newPas), (2f * newPas));
                                }
                            }
                        }
                        else if(result>-(newPas/2f))
                        {
                            computedCubes.Add(cubeCenter);
                            GameObject cubeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cubeObj.transform.parent = transform;
                            cubeObj.transform.position = cubeCenter;
                            cubeObj.transform.localScale = new Vector3((2f * newPas), (2f * newPas), (2f * newPas));
                        }
                    }
                }
            }
        }
    }

    
}
