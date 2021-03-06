using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    
    [HideInInspector] public List<Vector3> verticesList;
    List<GameObject> cubeList;

    List<Vector3> quadraticCurveList;
    List<Vector3> cubicCurveList;

    [HideInInspector] public List<Vector3> toComputeCurve;

    [SerializeField] private float pas;
    [SerializeField] private float width;
    [SerializeField] private Vector3 _point0 = new Vector3(-1f, 0f, 0f);
    [SerializeField] private Vector3 _point1 = new Vector3(-1f, 1f, 0f);
    [SerializeField] private Vector3 _point2 = new Vector3( 1f, 1f, 0f);
    [SerializeField] private Vector3 _point3 = new Vector3( 1f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        InitPoints();
    }

    

    public void InitPoints()
    {
        verticesList = new List<Vector3>();

        verticesList.Add(_point0);
        verticesList.Add(_point1);
        verticesList.Add(_point2);
        verticesList.Add(_point3);
    }


    public void ChangePointPos(int index, Vector3 newPos)
    {
        verticesList[index] = newPos;
    }

    public void AddPoint(Vector3 point2Add)
    {
        Vector3 controlPoint1 = verticesList[verticesList.Count-1] + (verticesList[verticesList.Count - 1]- verticesList[verticesList.Count - 2]);
        Vector3 controlPoint2 = new Vector3(point2Add.x + (verticesList[verticesList.Count - 1].x - verticesList[verticesList.Count - 2].x),
                                            point2Add.y + (verticesList[verticesList.Count - 1].y - verticesList[verticesList.Count - 2].y),
                                            point2Add.z + (verticesList[verticesList.Count - 1].z - verticesList[verticesList.Count - 2].z));
        //Add control point attached to the last point in list
        verticesList.Add(controlPoint1);
        //Add control point attached to the point to add
        verticesList.Add(controlPoint2);
        //Add the new point
        verticesList.Add(point2Add);
    }


    public void GenerateCurve()
    {
        //QuadraticCurve(verticesList[0], verticesList[1], verticesList[2]);
        //CubicCurveDeCasteljau(verticesList[0], verticesList[1], verticesList[2], verticesList[3]);
        //CubicCurveBernstein(verticesList[0], verticesList[1], verticesList[2], verticesList[3]);
        CubicCurveBernsteinAnyLength();
    }

    /*
     * Single Points methods
     */

    Vector3 Lerp(Vector3 p0, Vector3 p1, float t)
    {
        /*
         * Calculate the Linear Interpolation of 2 Vector3 Points.
         */
        Vector3 vecteurP0P1 = p1 - p0;
        Vector3 result = p0 + new Vector3(vecteurP0P1.x * t, vecteurP0P1.y * t, vecteurP0P1.z * t);
        return result;
    }

    Vector3 QuadraticPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        /*
         * Calculate the coordinates of a point following a Quadratic B?zier curve for a given t.
         */

        Vector3 newPoint0 = Lerp(p0, p1, t);
        Vector3 newPoint1 = Lerp(p1, p2, t);

        Vector3 quadraticPoint = Lerp(newPoint0, newPoint1, t);
        return quadraticPoint;
    }

    /*
     * All Points methods
     */

    void QuadraticCurve(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        /*
         * Calculate all the points following a Quadratic B?zier curve.
         */

        //Reset
        quadraticCurveList = new List<Vector3>();

        for (float k = 0f; k <= 1f; k += pas)
        {
            //Compute
            Vector3 quadraticPoint = QuadraticPoint(p0,p1,p2,k);

            //Add to list
            quadraticCurveList.Add(quadraticPoint);
        }

        //Send to master list
        toComputeCurve = quadraticCurveList;
    }

    void CubicCurveDeCasteljau(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        /*
         * Calculate all the points following a Cubic B?zier curve using De Casteljau's algorithm.
         */

        //Reset
        cubicCurveList = new List<Vector3>();

        for (float k = 0f; k <= 1f; k += pas)
        {
            //Compute
            Vector3 quadraticPoint1 = QuadraticPoint(p0, p1, p2, k);
            Vector3 quadraticPoint2 = QuadraticPoint(p1, p2, p3, k);
            Vector3 cubicPoint = Lerp(quadraticPoint1, quadraticPoint2, k);

            //Add to list
            cubicCurveList.Add(cubicPoint);
        }

        //Send to master list
        toComputeCurve = cubicCurveList;
    }


    void CubicCurveBernstein(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        /*
         * Calculate all the points following a B?zier curve, using Bernstein's Polynomials.
         */
        //Reset
        cubicCurveList = new List<Vector3>();

        for (float t = 0f ; t <= 1f ; t += pas)
        {
            //Init
            Vector3 cubicPoint = new Vector3();

            //Bernstein's Polynomials
            float p0Weight = -Mathf.Pow(t, 3f) + (3 * Mathf.Pow(t, 2f)) - 3 * t + 1;
            float p1Weight = 3 * Mathf.Pow(t, 3f) - 6 * Mathf.Pow(t, 2f) + 3 * t;
            float p2Weight = -3 * Mathf.Pow(t, 3f) + 3 * Mathf.Pow(t, 2f);
            float p3Weight = Mathf.Pow(t, 3f);

            // TODO : Check if the sum of the weights == 1f, else throw error
            // TODO : Get Normals

            //Compute Tangent
            Vector3 tangeant = getTangent(p0, p1, p2, p3, t);

            //Compute
            cubicPoint.x = (p0.x * (p0Weight)) + (p1.x * (p1Weight)) + (p2.x * (p2Weight)) + (p3.x * (p3Weight));
            cubicPoint.y = (p0.y * (p0Weight)) + (p1.y * (p1Weight)) + (p2.y * (p2Weight)) + (p3.y * (p3Weight));
            cubicPoint.z = (p0.z * (p0Weight)) + (p1.z * (p1Weight)) + (p2.z * (p2Weight)) + (p3.z * (p3Weight));

            //Add to list
            cubicCurveList.Add(cubicPoint);
        }

        //Send to master list
        toComputeCurve = cubicCurveList;
    }

    void CubicCurveBernsteinAnyLength()
    {
        /*
         * Calculate all the points following a B?zier curve, using Bernstein's Polynomials.
         */

        //Reset
        cubicCurveList = new List<Vector3>();

        for (int i = 0; i<(verticesList.Count-1); i+=3)
        {
            Vector3 p0 = verticesList[i];
            Vector3 p1 = verticesList[i+1];
            Vector3 p2 = verticesList[i+2];
            Vector3 p3 = verticesList[i+3];

            for (float t = 0f; t <= 1f; t += pas)
            {
                //Init
                Vector3 cubicPoint = new Vector3();

                //Bernstein's Polynomials
                float p0Weight = -Mathf.Pow(t, 3f) + (3 * Mathf.Pow(t, 2f)) - 3 * t + 1;
                float p1Weight = 3 * Mathf.Pow(t, 3f) - 6 * Mathf.Pow(t, 2f) + 3 * t;
                float p2Weight = -3 * Mathf.Pow(t, 3f) + 3 * Mathf.Pow(t, 2f);
                float p3Weight = Mathf.Pow(t, 3f);

                // TODO MAYBE: Check if the sum of the weights == 1f, else throw error
                // TODO : Get Normals
                
                //Compute Tangent, no use for now but yeah it's cool
                //Vector3 tangent = getTangent(p0, p1, p2, p3, t);

                //Compute
                cubicPoint.x = (p0.x * (p0Weight)) + (p1.x * (p1Weight)) + (p2.x * (p2Weight)) + (p3.x * (p3Weight));
                cubicPoint.y = (p0.y * (p0Weight)) + (p1.y * (p1Weight)) + (p2.y * (p2Weight)) + (p3.y * (p3Weight));
                cubicPoint.z = (p0.z * (p0Weight)) + (p1.z * (p1Weight)) + (p2.z * (p2Weight)) + (p3.z * (p3Weight));

                //Add to list
                cubicCurveList.Add(cubicPoint);
            }
        }
        //Send to master list
        toComputeCurve = cubicCurveList;

        //Add The Forgotten Onne
        toComputeCurve.Add(verticesList[verticesList.Count - 1]);
    }

    Vector3 getTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        /*
         * Calculate the Tangent (Normalized Velocity) of a point following a B?zier Curve.
         */
    
        //Init
        Vector3 tangent = new Vector3();

        //Bernstein's Polynomials
        float p0Weight = - (3 * Mathf.Pow(t, 2f)) + 6 * t - 3;
        float p1Weight =  9 * Mathf.Pow(t, 2f) - 12 * t + 3;
        float p2Weight = - 9 * Mathf.Pow(t, 2f) + 6 * t;
        float p3Weight = 3 * Mathf.Pow(t, 2f);

        //Compute
        tangent.x = (p0.x * (p0Weight)) + (p1.x * (p1Weight)) + (p2.x * (p2Weight)) + (p3.x * (p3Weight));
        tangent.y = (p0.y * (p0Weight)) + (p1.y * (p1Weight)) + (p2.y * (p2Weight)) + (p3.y * (p3Weight));
        tangent.z = (p0.z * (p0Weight)) + (p1.z * (p1Weight)) + (p2.z * (p2Weight)) + (p3.z * (p3Weight));

        tangent.Normalize();

        return tangent;
    }

    
}
