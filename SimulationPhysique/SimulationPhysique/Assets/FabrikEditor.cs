using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Fabrik))]
public class FabrikEditor : Editor
{
    
    Fabrik fabrik;

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown & guiEvent.button == 0 & guiEvent.shift)
        {
            Vector3 mousePos3 = new Vector3(mousePos.x, mousePos.y, 0f);

            Undo.RecordObject(fabrik, "ChangeTargetPos");
            fabrik.ChangeTargetPos(mousePos3);
        }
    }

    void Draw()
    {
        //fabrik.FabrikUpdate();
        Handles.color = Color.red;
        for (int i = 0; i < fabrik.pointList.Count-1; i++)
        {
            Vector3 newPos = Handles.FreeMoveHandle(fabrik.pointList[i], Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);
        }

        Vector3 target = Handles.FreeMoveHandle(fabrik.pointList[fabrik.pointList.Count - 1], Quaternion.identity, 0.1f, Vector3.zero, Handles.SphereHandleCap);

        Handles.color = Color.green;
        Handles.DrawPolyLine(fabrik.pointList.ToArray());
        
    }

    private void OnEnable()
    {
        fabrik = (Fabrik)target;
        fabrik.Init();
    }
    
}
