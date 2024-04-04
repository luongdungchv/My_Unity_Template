using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

public class CustomEdgeGUI : EdgeGUI
{
    private HashSet<(Node, Node)> mapper = new HashSet<(Node, Node)>();
    public void RenderEdges()
    {
        var edgeList = ((this.host as StateMachineGraph).graph as CustomGraph).FSMEdgeList;
        Debug.Log(Event.current.type);
        foreach (var edge in edgeList)
        {
            var start = edge.StartNode.position.center;
            if (edge.EndNode == null && TransitionMaker.IsMakingTransition)
            {
                var endPosition = Event.current.mousePosition;
                
                Handles.DrawLine(start, endPosition);
                continue;
            }
            var end = edge.EndNode.position.center;
            if (mapper.Contains((edge.StartNode, edge.EndNode)))
            {
                Handles.DrawLine(start, end);
                continue;
            };
            if (mapper.Contains((edge.EndNode, edge.StartNode)))
            {
                var dir = (end - start).normalized;
                var normal = new Vector2(-dir.y, dir.x);
                start += normal * 10;
                end += normal * 10;
            }
            else
            {
                mapper.Add((edge.StartNode, edge.EndNode));
            }
            Handles.DrawLine(start, end);
        }
        if (Event.current.type == EventType.MouseMove)
            Event.current.Use();
    }

}
