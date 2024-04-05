using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.Graphs;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.CodeDom;
using System;
using System.Reflection;

public class StateMachineWindow : EditorWindow
{
    private StateMachineGraph graphGUI;
    private CustomGraph graph;

    public CustomGraph GraphModel => this.graph;
    public StateMachineGraph GraphGUI => this.graphGUI;

    public UnityAction OnUpdate;

    private IMGUIContainer guiContainer;
    private Rect windowArea;
    private Manipulator manipulator;
    private float zoomLevel = 1;

    [OnOpenAsset(0)]
    public static bool ShowWindow(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID);
        if (asset is StateMachineDataSO)
        {
            var window = GetWindow<StateMachineWindow>("Test");
            window.wantsMouseMove = true;
            window.wantsMouseEnterLeaveWindow = true;
            window.Show();
            Debug.Log(window.position);
            window.SetUpGraph();
        }


        return false;
    }

    public void SetUpGraph()
    {
        graph = ScriptableObject.CreateInstance<CustomGraph>();
        graph.hideFlags = HideFlags.HideAndDontSave;

        //create new node
        CustomNode node = ScriptableObject.CreateInstance<CustomNode>();
        node.title = "mile";
        node.position = new Rect(0, 0, 0, 0);

        var node2 = ScriptableObject.CreateInstance<CustomNode>();
        node2.title = "node2";
        node2.position = new Rect(60, 60, 0, 0);

        node.AddProperty(new Property(typeof(MonoScript), "integer"));
        graph.AddNode(node);
        graph.AddNode(node2);

        //create edge
        //graph.Connect(node, node2);

        graphGUI = ScriptableObject.CreateInstance<StateMachineGraph>();
        graphGUI.graph = graph;

        // this.guiContainer = new IMGUIContainer(this.Draw){
        //     name = "GraphUI",
        //     style = {
        //         overflow = Overflow.Visible
        //     }
        // };

        //base.rootVisualElement.Add(guiContainer);
        this.windowArea = this.position;
        this.manipulator = new Manipulator();
        this.manipulator.SetUp(windowArea);
        Debug.Log(this.windowArea);
        this.zoomLevel = 1;
    }

    private void OnGUI()
    {
        if (graphGUI != null)
        {
            //Debug.Log(this.position);
            graphGUI.BeginGraphGUI(this, new Rect(0, 0, this.position.width, this.position.height));
            GUI.EndScrollView();
            manipulator.BeginGUI();
            this.HandlePan();
            graphGUI.OnGraphGUI();
            graphGUI.EndGraphGUI();
            manipulator.EndGUI();
            
            this.HandleScroll();
        }
    }
    private void HandleScroll()
    {
        if (Event.current.type == EventType.ScrollWheel)
        {
            var zoomAmount = Event.current.delta.y / Math.Abs(Event.current.delta.y);
            var lastZoomLevel = zoomLevel;
            this.zoomLevel -= zoomAmount / 8;
            zoomLevel = Mathf.Clamp(zoomLevel, 0.25f, 2.25f);

            var ratio = zoomLevel / lastZoomLevel;

            manipulator.SetZoomScale(zoomLevel);
            var dir = manipulator.position - Event.current.mousePosition;
            dir *= ratio;
            manipulator.SetRectPosition(Event.current.mousePosition + dir);

            Event.current.Use();
        }
    }
    private void HandlePan()
    {
        
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 2)
        {
            Debug.Log("Drag");
            var delta = Event.current.delta;
            this.manipulator.Translate(delta);
            Event.current.Use();

        }
    }
    // private void Update() {
    //     this.graphGUI.OnUpdate();
    // }
}
