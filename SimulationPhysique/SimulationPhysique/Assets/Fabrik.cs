using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabrik : MonoBehaviour
{
    //LineRenderer lr;
    [HideInInspector] public List<Vector3> pointList;

    List<float> segmentSizeList;
    List<float> constraintList;
    Vector3 anchor;
    

    Vector3 target;

    public void Init()
    {
        
        pointList = new List<Vector3>();
        pointList.Add(new Vector3(1, 0, 0));
        pointList.Add(new Vector3(1, 0.5f, 0));
        pointList.Add(new Vector3(-1, 1, 0));
        pointList.Add(new Vector3(-1, 0, 0));

        segmentSizeList = new List<float>();
        for (int i = 0; i < (pointList.Count - 1); i++)
        {
            segmentSizeList.Add(Vector3.Distance(pointList[i], pointList[i + 1]));
        }

        constraintList = new List<float>();
        constraintList.Add(-2f); //anchor isn't blocked
        constraintList.Add(0.5f);
        constraintList.Add(0.5f);

        anchor = pointList[0];
    }

    public void ChangeTargetPos(Vector3 newPos)
    {
        target = newPos;
        FabrikAlgo();
    }

   

    public void FabrikAlgo()
    {
        Vector3 targetTemp = target;
        for(int j =0; j< 50; j++) //maxIterations needs to be pair in order to have the last iteration going to anchor
        {
            //Last point of list is done "by hand"
            pointList[pointList.Count - 1] = targetTemp;

            for (int i = (pointList.Count - 2); i >= 0; i--)
            {
                Vector3 translation = pointList[i] - pointList[i + 1];
                translation.Normalize();

                Vector3 tangent = new Vector3();
                if(i-1<=0)
                {
                    tangent = Vector3.up;
                }
                else
                {
                    tangent = pointList[i] - pointList[i - 1];
                }
                
                tangent.Normalize();
                Vector3 direction = pointList[i + 1] - pointList[i];
                direction.Normalize();

                float dotProd = Vector3.Dot(tangent, direction);
                if(dotProd < constraintList[i])
                {
                    //float sin = Mathf.Sqrt((1-Mathf.Pow(constraintList[i], 2f)));
                    float sin = Mathf.Acos(constraintList[i]);
                    Vector3 newVector = new Vector3();
                    newVector.x = constraintList[i];
                    newVector.y = sin;
                    pointList[i] = pointList[i + 1] + (newVector * segmentSizeList[i]);     
                }
                else
                {
                    pointList[i] = pointList[i + 1] + (translation * segmentSizeList[i]);
                }
            }

            if(j%2 == 0)
            {
                targetTemp = anchor;
            }
            else
            {
                targetTemp = target;
            }
            constraintList.Reverse();
            segmentSizeList.Reverse();
            pointList.Reverse();
        }
    } 
}
