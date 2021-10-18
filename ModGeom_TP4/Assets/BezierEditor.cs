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
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if(guiEvent.type == EventType.MouseDown & guiEvent.button==0 & guiEvent.shift)
        {
            Vector3 mousePos3 = new Vector3(mousePos.x, mousePos.y, bezier.verticesList[bezier.verticesList.Count - 1].z);
            Undo.RecordObject(bezier, "AddPoint");
            bezier.AddPoint(mousePos3);
        }
    }

    void Draw()
    {
        Handles.color = Color.red;
        for(int i = 0; i < bezier.verticesList.Count; i++)
        {
            Vector3 newPos = Handles.FreeMoveHandle(bezier.verticesList[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
            if(bezier.verticesList[i] != newPos)
            {
                Undo.RecordObject(bezier, "ChangePointPos");
                bezier.ChangePointPos(i, newPos);
            }
        }

        bezier.GenerateCurve();
        Handles.color = Color.green;
        Handles.DrawPolyLine(bezier.toComputeCurve.ToArray());
    }


    void OnEnable()
    {
        bezier = (Bezier)target;
        bezier.InitPoints();
    }
}
