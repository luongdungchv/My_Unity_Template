using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using System.Reflection;
using System;

public class CustomEdgeGUI : EdgeGUI
{
    private HashSet<(Node, Node)> mapper = new HashSet<(Node, Node)>();
    private float arrowSize = 5f;
    public void RenderEdges()
    {
        var edgeList = ((this.host as StateMachineGraph).graph as CustomGraph).FSMEdgeList;
        foreach (var edge in edgeList)
        {
            var start = edge.StartNode.position.center;
            if (edge.EndNode == null && TransitionMaker.IsMakingTransition)
            {
                var endPosition = Event.current.mousePosition;

                Handles.DrawLine(start, endPosition);
                this.RenderArrow(start, endPosition);
                continue;
            }
            var end = edge.EndNode.position.center;
            if (mapper.Contains((edge.StartNode, edge.EndNode)))
            {
                Handles.DrawLine(start, end);
                this.RenderArrow(start, end);
                continue;
            };
            if (mapper.Contains((edge.EndNode, edge.StartNode)))
            {
                var dir = (end - start).normalized;
                var normal = new Vector2(-dir.y, dir.x);
                start += normal * 15;
                end += normal * 15;
            }
            else
            {
                mapper.Add((edge.StartNode, edge.EndNode));
            }
            this.RenderArrow(start, end);
            Handles.DrawLine(start, end);
        }
        if (Event.current.type == EventType.MouseMove)
            Event.current.Use();
    }
    private void RenderArrow(Vector2 start, Vector2 end)
    {
        if (Event.current.type == EventType.Repaint)
        {
            var center = (start + end) / 2;
            var direction = (end - start).normalized;
            var color = Color.white;
            var cross = new Vector2(-direction.y, direction.x);
            cross = cross.normalized;

            Vector3[] array = new Vector3[4];
            array[0] = center + direction * arrowSize;
            array[1] = center - direction * arrowSize + cross * arrowSize;
            array[2] = center - direction * arrowSize - cross * arrowSize;
            array[3] = array[0];
            Shader.SetGlobalColor("_HandleColor", color);
            //typeof(HandleUtility).GetMethod("ApplyWireMaterial", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[0]);
            typeof(HandleUtility).InvokeMember("ApplyWireMaterial", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, new object[0]);
            //HandleUtility.ApplyWireMaterial();
            GL.Begin(4);
            GL.Color(color);
            GL.Vertex(array[0]);
            GL.Vertex(array[1]);
            GL.Vertex(array[2]);
            GL.End();
            Handles.color = color;
            Handles.DrawAAPolyLine((Texture2D)Styles.connectionTexture.image, 1, array);
        }
    }

}
