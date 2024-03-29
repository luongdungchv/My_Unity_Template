using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class StateMachineGraphView : GraphView
{
    private string styleSheetName = "GraphViewStyleSheet";
    public StateMachineWindow window;

    private NodeSearchWindow searchWindow;

    public StateMachineGraphView(StateMachineWindow window){
        this.window = window;
        styleSheets.Add(Resources.Load<StyleSheet>(styleSheetName));

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        this.AddSearchWindow();

        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        //AddSearchWindow();
    }

    private void AddSearchWindow(){
        this.searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        this.searchWindow.Configure(window, this, null);
        nodeCreationRequest = ctx => SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), searchWindow);
    }

    public BaseNode CreateNode(Vector2 pos, bool isLoad){
        var res = new BaseNode(pos, window, this);
        if (!isLoad) res.AddOutputPort("Next Group", Port.Capacity.Single);
        return res;
        
    }
}
