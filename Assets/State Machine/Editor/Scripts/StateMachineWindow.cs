using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor.Graphs;
using UnityEngine.Events;

public class StateMachineWindow : EditorWindow
{
    private StateMachineGraph graphGUI;
    private CustomGraph graph;

    public CustomGraph GraphModel => this.graph;
    public StateMachineGraph GraphGUI => this.graphGUI;

    public UnityAction OnUpdate;

    [OnOpenAsset(0)]
    public static bool ShowWindow(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID);
        if (asset is StateMachineDataSO)
        {
            var window = GetWindow<StateMachineWindow>("Test");
            window.wantsMouseMove = true;
            window.Show();
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
    }

    private void OnGUI() {
        if(graphGUI != null){
            graphGUI.BeginGraphGUI(this, new Rect(0, 0, this.position.width, this.position.height));
            graphGUI.OnGraphGUI();
            graphGUI.EndGraphGUI();
        }
    }
    private void Update() {
        this.graphGUI.OnUpdate();
    }
}
