using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

public class CustomGraph : Graph
{
    private List<CustomEdge> fsmEdgeList = new List<CustomEdge>();
    public List<CustomEdge> FSMEdgeList => fsmEdgeList;
    public void Connect(Node startNode, Node endNode){
        var edge = new CustomEdge(startNode, endNode);
        this.fsmEdgeList.Add(edge);
    } 
    public void AddEdge(CustomEdge edge){
        this.fsmEdgeList.Add(edge);
    }
    public void RemoveLastEdge(){
        if(this.fsmEdgeList.Count == 0) return;
        this.fsmEdgeList.RemoveAt(this.fsmEdgeList.Count - 1);
    }
}
