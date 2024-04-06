using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Graphs;
using UnityEditor;

public class CustomNode : Node
{
    [SerializeField] private List<MonoScript> scriptAssetList;
    [SerializeField] private string nodeTitle;

    public List<MonoScript> ScriptAssets => this.scriptAssetList; 

    public override string title { get => nodeTitle; set => nodeTitle = value; }
}
