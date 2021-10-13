using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    [SerializeField] private float pas;
    List<Vector3> verticesList;
    List<GameObject> cubeList;

    List<Vector3> quadraticCurveList;
    List<Vector3> cubicCurveList;

    List<Vector3> toComputeCurve;
    // Start is called before the first frame update
    void Start()
    {
        verticesList = new List<Vector3>();
        cubeList = new List<GameObject>();

        GenerateLine();
        GenerateCubes();
        //QuadraticCurve();
        CubicCurve();

        toComputeCurve.Add(verticesList[verticesList.Count - 1]);

        gameObject.AddComponent<LineRenderer>();
        gameObject.GetComponent<LineRenderer>().positionCount = toComputeCurve.Count;
        gameObject.GetComponent<LineRenderer>().startWidth = 0.17f;
        gameObject.GetComponent<LineRenderer>().endWidth = 0.17f;
        gameObject.GetComponent<LineRenderer>().SetPositions(toComputeCurve.ToArray());
    }

    void GenerateLine()
    {
        verticesList.Add(new Vector3(1, 2, 0));
        verticesList.Add(new Vector3(-2, 1, 0));
        verticesList.Add(new Vector3(-1, -1, 0));
        verticesList.Add(new Vector3(1, -1, 0));
    }

    void GenerateCubes()
    {
        for( int i =0; i< verticesList.Count; i++)
        {
           GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
           cube.transform.position = verticesList[i];
           cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
           cube.transform.parent = gameObject.transform;
        }
        
    }

    void QuadraticCurve()
    {
        quadraticCurveList = new List<Vector3>();
        Vector3 p0 = verticesList[0];
        Vector3 p1 = verticesList[1];
        Vector3 p2 = verticesList[2];

        for (float k = 0f; k <= 1f; k += pas)
        {
            Vector3 quadraticPoint = QuadraticPoint(p0,p1,p2,k);

            quadraticCurveList.Add(quadraticPoint);
        }
        toComputeCurve = quadraticCurveList;
    }

    void CubicCurve()
    {
        cubicCurveList = new List<Vector3>();
        Vector3 p0 = verticesList[0];
        Vector3 p1 = verticesList[1];
        Vector3 p2 = verticesList[2];
        Vector3 p3 = verticesList[3];

        for (float k = 0f; k <= 1f; k += pas)
        {
            Vector3 quadraticPoint1 = QuadraticPoint(p0, p1, p2, k);
            Vector3 quadraticPoint2 = QuadraticPoint(p1, p2, p3, k);

            Vector3 cubicPoint = Lerp(quadraticPoint1, quadraticPoint2, k);

            cubicCurveList.Add(cubicPoint);
        }

        toComputeCurve = cubicCurveList;
    }

    Vector3 QuadraticPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        Vector3 newPoint0 = Lerp(p0, p1, t);
        Vector3 newPoint1 = Lerp(p1, p2, t);

        Vector3 quadraticPoint = Lerp(newPoint0, newPoint1, t);
        return quadraticPoint;
    }

    Vector3 Lerp(Vector3 p0, Vector3 p1, float t)
    {
        Vector3 vecteurP0P1 = p1 - p0;
        Vector3 result = p0 + new Vector3(vecteurP0P1.x * t, vecteurP0P1.y * t, vecteurP0P1.z * t);
        return result;
    }
}
