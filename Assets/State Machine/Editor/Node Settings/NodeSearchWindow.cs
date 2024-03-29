using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

using UnityEditor.Experimental.GraphView;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private StateMachineWindow window;
    private StateMachineGraphView graph;
    private Port startPort;

    public void Configure(StateMachineWindow window, StateMachineGraphView graph, Port startPort)
    {
        this.window = window;
        this.graph = graph;
        this.startPort = startPort;
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        List<SearchTreeEntry> entries = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Content Node"), 0),
            AddNodeSearch("State", new BaseNode()),
        };

        return entries;
    }
    private SearchTreeEntry AddNodeSearch(string _name, BaseNode _basenode)
    {
        SearchTreeEntry res = new SearchTreeEntry(new GUIContent(_name))
        {
            level = 1,
            userData = _basenode
        };
        return res;
    }
    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        Vector2 mousePos = window.rootVisualElement.ChangeCoordinatesTo
            (window.rootVisualElement.parent, context.screenMousePosition - window.position.position);
        Vector2 graphMousePos = graph.contentViewContainer.WorldToLocal(mousePos);
        return CheckNodeType(SearchTreeEntry, graphMousePos);
    }
    private bool CheckNodeType(SearchTreeEntry entry, Vector2 pos)
    {
       
        if(entry.userData.GetType() == typeof(BaseNode)){
            var node = graph.CreateNode(pos, false);
            graph.AddElement(node);
            ConnectNode(node.inputPortList[0]);
            return true;
        }
       
        return false;
    }
    void ConnectNode(Port endPort)
    {
        if (startPort != null) graph.AddElement(startPort.ConnectTo(endPort));
    }
}
