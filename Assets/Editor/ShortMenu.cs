using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ShortMenu : Editor
{
    [MenuItem("Open Scenes/Test1")]//0
    public static void Option1(){
        string scenePath = "Assets/Scenes/Test1.unity";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
    [MenuItem("Open Scenes/Test2")]//1
    public static void Option2(){
        string scenePath = "Assets/Scenes/Test2.unity";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
    [MenuItem("Open Scenes/SampleScene")]//2
    public static void Option3(){
        string scenePath = "Assets/Scenes/SampleScene.unity";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
    [MenuItem("Open Scenes/SampleScene")]//3
    public static void Option4(){
        string scenePath = "Assets/Scenes/SampleScene.unity";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
    //menu item placeholder4
    public static void Option5(){
        string scenePath = "scene name placeholder4";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
    //menu item placeholder5
    public static void Option6(){
        string scenePath = "scene name placeholder5";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
}
