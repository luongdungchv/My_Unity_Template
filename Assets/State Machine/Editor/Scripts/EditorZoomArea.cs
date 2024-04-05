using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorZoomArea
{
    private const float kEditorWindowTabHeight = 21.0f;
    private static Matrix4x4 _prevGuiMatrix;

    private static Vector2 mousePos = new Vector2(0, kEditorWindowTabHeight);


    public static Rect Begin(float zoomAmount, float zoomScale, Rect screenCoordsArea)
    {
        GUI.EndGroup();

        var clippedArea = screenCoordsArea;

        if (Event.current.type == EventType.ScrollWheel)
        {
            mousePos = Event.current.mousePosition;
            Debug.Log(mousePos);
            clippedArea = clippedArea.ScaleSizeBy(zoomAmount, mousePos);
        }
        
        GUI.BeginGroup(clippedArea);

        _prevGuiMatrix = GUI.matrix;
        Matrix4x4 translation = Matrix4x4.TRS(clippedArea.TopLeft(), Quaternion.identity, Vector3.one);
        Matrix4x4 scale = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1.0f));
        GUI.matrix = translation * scale * translation.inverse * GUI.matrix;
        
        return clippedArea;
    }

    public static void End()
    {
        GUI.matrix = _prevGuiMatrix;
        
        GUI.EndGroup();
        GUI.BeginGroup(new Rect(0.0f, kEditorWindowTabHeight, Screen.width, Screen.height));
    }
}
