using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaikinSubdivision : MonoBehaviour
{
    [SerializeField] private int nb_recursions;
    List<Vector3> verticesList;
    List<Vector3> newVertices;
    List<Vector3> temp;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<LineRenderer>();

        verticesList = new List<Vector3>();

        GenerateLine();
        Chaikin();

        gameObject.GetComponent<LineRenderer>().positionCount = newVertices.Count;
        gameObject.GetComponent<LineRenderer>().startWidth = 0.17f;
        gameObject.GetComponent<LineRenderer>().endWidth = 0.17f;
        gameObject.GetComponent<LineRenderer>().loop = true;
        gameObject.GetComponent<LineRenderer>().SetPositions(newVertices.ToArray());
    }

    

    void GenerateLine()
    {
        verticesList.Add(new Vector3(1, 1, 0));
        verticesList.Add(new Vector3(0, 5, 0));
        verticesList.Add(new Vector3(-1, 1, 0));
        verticesList.Add(new Vector3(-5, 0, 0));
        verticesList.Add(new Vector3(-1, -1, 0));
        verticesList.Add(new Vector3(0, -5, 0));
        verticesList.Add(new Vector3(1, -1, 0));
        verticesList.Add(new Vector3(5, 0, 0));
    }

    void Chaikin()
    {
        newVertices = verticesList;

        for (int i = 0; i<nb_recursions; i++ )
        {
            temp = new List<Vector3>();

            for (int j =0; j<newVertices.Count;j++)
            {
                Vector3 p0 = newVertices[j];

                Vector3 p1 = new Vector3();
                if((j+1)>=newVertices.Count)
                {
                    p1 = newVertices[0];
                }
                else
                {
                    p1 = newVertices[j + 1];
                }

                Vector3 vecteurP0P1 = p1 - p0;
                Vector3 newPoint1 = p0 + new Vector3(vecteurP0P1.x * 0.25f, vecteurP0P1.y * 0.25f, vecteurP0P1.z * 0.25f);
                Vector3 newPoint2 = p0 + new Vector3(vecteurP0P1.x * 0.75f, vecteurP0P1.y * 0.75f, vecteurP0P1.z * 0.75f);

                temp.Add(newPoint1);
                temp.Add(newPoint2);
            }
            newVertices = temp;
        }
    }
}
