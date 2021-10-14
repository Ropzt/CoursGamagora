using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bezier))]
public class BezierEditor : Editor
{
    Bezier bezier;
    private void OnSceneGUI()
    {
        
    }

    void Draw()
    {
        Handles.color = Color.red;
        for(int i = 0; i < bezier.verticesList.Count; i++)
        {
            /*Vector3 newPos = Handles.FreeMoveHandle();
            if(bezier.verticesList[i] != newPos)
            {

            }
            */
        }
    }


    void OnEnable()
    {
        bezier = (Bezier)target;
        bezier.ResetList();
    }
}
