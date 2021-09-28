using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Globalization;

public class MaillageLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] lines = File.ReadAllLines(@"C:\Users\pariz\Documents\Github\CoursGamagora\ModGeomTP2\ModGeomTP2\Assets\buddha.off");

        //Line 1 
        string[] line1Split = lines[1].Split(' ');
        int.TryParse(line1Split[0], out int nb_vertices);
        int.TryParse(line1Split[1], out int nb_facettes);

        List<Vector3> verticesList = new List<Vector3>();

        for(int i =0; i< 5; i++)
        {
            string[] lineVerticesCoord = lines[2+i].Split(' ');

            Debug.Log("X = " + lineVerticesCoord[0]);
            double x = double.Parse(lineVerticesCoord[0], NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint);
            Debug.Log("X = " + x);
            /*
            double.TryParse(lineVerticesCoord[0], out double x);
            Debug.Log("X = " + x);
            double.TryParse(lineVerticesCoord[1], out double y);
            Debug.Log("Y = " + y);
            double.TryParse(lineVerticesCoord[2], out double z);
            Debug.Log("Z = " + z);
            verticesList.Add(new Vector3((float)x, (float)y, (float)z));*/
        }



        //Debug.Log("Line 0 = " + verticesList[0]);


        //Debug.Log("Line 0 = " + line1Split[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
