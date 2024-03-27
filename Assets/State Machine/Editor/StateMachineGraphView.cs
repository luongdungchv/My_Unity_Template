using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class StateMachineGraphView : GraphView
{
    private string styleSheetName = "GraphViewStyleSheet";
    public StateMachineWindow window;

    public StateMachineGraphView(StateMachineWindow window){
        this.window = window;
        styleSheets.Add(Resources.Load<StyleSheet>(styleSheetName));

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        //AddSearchWindow();
    }

    public void CreateNode(){
        var node = new BaseNode();
        
    }
}
