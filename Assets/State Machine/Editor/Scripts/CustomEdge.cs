using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

public class CustomEdge
{
    private Node startNode, endNode;

    public Node StartNode => this.startNode;
    public Node EndNode => this.endNode;

    public CustomEdge(Node startNode, Node endNode){
        this.startNode = startNode;
        this.endNode = endNode;
    }

    public CustomEdge(){

    }

    public void SetStartNode(Node startNode){
        this.startNode = startNode;
    }
    public void SetEndNode(Node endNode){
        this.endNode = endNode;
    }
}
