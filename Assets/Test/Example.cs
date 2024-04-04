using UnityEngine;

public class Example : MonoBehaviour
{
    // Draws a line from the bottom right
    // to the top left of the Screen
    // Using GL.Vertex
    [SerializeField] private Material mat;

    void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.LINES);
        GL.Vertex(new Vector3(1, 0, 0));
        GL.Vertex(new Vector3(0, 1, 0));
        GL.End();
        GL.PopMatrix();
    }
    [Sirenix.OdinInspector.Button]
    private void Test(){
        var t1 = new TestClass();
        var t2 = new TestClass();
        var b1 = (t1, t2);
        var b2 = (t1, t2);
        var b3 = (t2, t1);
        Debug.Log( b1 == b3);
    }
    class TestClass{

    }
}