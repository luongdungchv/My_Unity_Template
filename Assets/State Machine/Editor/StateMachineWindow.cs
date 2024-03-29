using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Graphs;

public class StateMachineWindow : EditorWindow
{
    private StateMachineGraphView graph;

    [OnOpenAsset(0)]
    public static bool ShowWindow(int instanceID, int line){
        var asset = EditorUtility.InstanceIDToObject(instanceID);
        if(asset is StateMachineDataSO){
            var window = GetWindow<StateMachineWindow>();
            window.GenerateGraph();
            return true;
        }
        return false;
    }

    public void SetUp(){
        rootVisualElement.Clear();

    }
    public void GenerateGraph(){
        graph = new StateMachineGraphView(this);
        graph.StretchToParentSize();
        rootVisualElement.Add(graph);
    }
}
