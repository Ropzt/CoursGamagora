using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRange : MonoBehaviour
{
    [SerializeField] private float nb_points;
    [SerializeField] private float turnFraction;
    public List<Vector3> rayDir;

    // Start is called before the first frame update
    void Start()
    {
        GenerateDirVectors();
    }

    void GenerateDirVectors()
    {
        rayDir = new List<Vector3>();
        for(int i = 0; i<nb_points; i++)
        {
            //float t = (i / (nb_points - 1)/(180/_fov));
            float t = i / (nb_points - 1f);
            float teta = 2 * Mathf.PI * turnFraction * i;
            float phi = Mathf.Acos((1 - 2 * t));
            
            float x = Mathf.Cos(teta) * Mathf.Sin(phi);
            float y = Mathf.Sin(teta) * Mathf.Sin(phi);
            float z = Mathf.Cos(phi);

            rayDir.Add( new Vector3(x, y, z));
        } 
    }
}
