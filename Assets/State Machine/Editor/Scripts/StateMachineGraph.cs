using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;
using UnityEditor;
using UnityEngine.Profiling;
using System.Reflection;

public class StateMachineGraph : GraphGUI
{
    private TransitionMaker transitionMaker = new TransitionMaker();
    public override IEdgeGUI edgeGUI
    {
        get
        {
            bool flag = this.m_EdgeGUI == null;
            if (flag)
            {
                this.m_EdgeGUI = new CustomEdgeGUI
                {
                    host = this
                };
            }
            return this.m_EdgeGUI;
        }
    }
    public void OnUpdate()
    {

    }

    public new void BeginGraphGUI(EditorWindow host, Rect position)
    {
        this.m_GraphClientArea = position;
        this.m_Host = host;
        bool flag = Event.current.type == EventType.Repaint;

        var graphExtents = (Rect)typeof(Graph).GetField("graphExtents", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.graph);
        //Debug.Log(typeof(Graph).GetField("graphExtents", BindingFlags.NonPublic | BindingFlags.Instance));
        if (flag)
        {
            Styles.graphBackground.Draw(position, false, false, false, false);
        }
        //this.m_ScrollPosition = GUI.BeginScrollView(position, this.m_ScrollPosition, graphExtents, GUIStyle.none, GUIStyle.none);
        bool flag2 = Event.current.type == EventType.Repaint;
        if (flag2)
        {
            Vector2 vector = -this.m_ScrollPosition - graphExtents.min;
            Rect gridRect = new Rect(-vector.x, -vector.y, position.width, position.width);

            var drawGridInfo = typeof(GraphGUI).GetMethod("DrawGrid", BindingFlags.NonPublic | BindingFlags.Instance);
            drawGridInfo.Invoke(this, new object[]{gridRect, 1f});
        }
    }

    public override void NodeGUI(Node n)
    {
        GUILayoutUtility.GetRect(200, 40);
        base.SelectNode(n);
        if (TransitionMaker.IsMakingTransition && Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 1)
        {
            foreach (var x in (this.graph as CustomGraph).FSMEdgeList)
            {
                if (x.StartNode == transitionMaker.StartNode && x.EndNode == n) return;
            }
            (this.graph as CustomGraph).RemoveLastEdge();
            (this.graph as CustomGraph).Connect(transitionMaker.StartNode, n);
            transitionMaker.StopMakingTransition();
        }
        n.NodeUI(this);
        base.DragNodes();
    }
    public override void OnGraphGUI()
    {
        this.m_Host.BeginWindows();
        bool clickOnNode = false;
        this.DisplayContextMenu();
        foreach (CustomNode node in this.m_Graph.nodes)
        {
            Node n2 = node;
            bool on = this.selection.Contains(node);
            Styles.Color color = node.nodeIsInvalid ? Styles.Color.Red : node.color;
            node.position = GUILayout.Window(
                node.GetInstanceID(),
                node.position,
                (i) =>
                {
                    this.NodeGUI(n2);
                    clickOnNode = false;
                },
                node.title, Styles.GetNodeStyle(node.style, color, on)
            );
        }
        (this.edgeGUI as CustomEdgeGUI).RenderEdges();
        //this.edgeGUI.DoDraggedEdge();
        this.m_Host.EndWindows();
        this.DragSelection();
        this.HandleMenuEvents();

        if (!clickOnNode) this.HandleMakeTransition();
    }

    private void DisplayContextMenu()
    {
        bool flag = Event.current.type != EventType.MouseDown || Event.current.button != 1 || Event.current.clickCount != 1;
        if (!flag)
        {
            Event.current.Use();
            Vector2 mousePosition = Event.current.mousePosition;
            Rect position = new Rect(mousePosition.x, mousePosition.y, 0f, 0f);
            List<GUIContent> list = new List<GUIContent>();
            bool flag2 = this.selection.Count != 0;
            if (flag2)
            {
                list.Add(EditorGUIUtility.TrTextContent("Make Transition", null));
                list.Add(EditorGUIUtility.TrTextContent("Cut", null));
                list.Add(EditorGUIUtility.TrTextContent("Copy", null));
                list.Add(EditorGUIUtility.TrTextContent("Duplicate"));
                list.Add(new GUIContent(string.Empty));
                list.Add(EditorGUIUtility.TrTextContent("Delete"));
            }
            else
            {
                list.Add((this.edgeGUI.edgeSelection.Count == 0) ? EditorGUIUtility.TrTextContent("Paste", null) : EditorGUIUtility.TrTextContent("Delete", null, ""));
            }
            GUIContent[] options = list.ToArray();
            ContextMenuData userData = new ContextMenuData
            {
                items = list.ToArray(),
                mousePosition = mousePosition
            };
            this.m_contextMenuMouseDownPosition = default(Vector2?);
            EditorUtility.DisplayCustomMenu(position, options, -1, new EditorUtility.SelectMenuItemFunction(this.ContextMenuClick), userData);
        }


    }

    private void ContextMenuClick(object userData, string[] options, int selected)
    {
        bool flag = selected < 0;
        if (!flag)
        {
            ContextMenuData contextMenuData = (ContextMenuData)userData;
            string eventText = contextMenuData.items[selected].text;
            if (eventText != null)
            {

                if (eventText == "Paste")
                {
                    this.m_contextMenuMouseDownPosition = new Vector2?(contextMenuData.mousePosition);
                    this.m_Host.SendEvent(EditorGUIUtility.CommandEvent(eventText));
                }

                else if (eventText == "Make Transition")
                {
                    var selectedNode = this.selection[0];
                    this.transitionMaker.SetStartNode(selectedNode);
                    this.transitionMaker.StartMakingTransition();
                    this.m_Host.SendEvent(EditorGUIUtility.CommandEvent(eventText));

                    var tempEdge = new CustomEdge();
                    tempEdge.SetStartNode(selectedNode);
                    (this.graph as CustomGraph).AddEdge(tempEdge);
                }

                else
                {
                    this.m_Host.SendEvent(EditorGUIUtility.CommandEvent(eventText));
                }
            }
        }
    }

    private void HandleMakeTransition()
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 1 && TransitionMaker.IsMakingTransition)
        {
            (this.graph as CustomGraph).RemoveLastEdge();
            this.transitionMaker.StopMakingTransition();
        }
    }

    class ContextMenuData
    {
        public GUIContent[] items;
        public Vector2 mousePosition;
    }
}
