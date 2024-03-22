using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class StateMachineWindow : EditorWindow
{
    [OnOpenAsset(0)]
    public static void ShowWindow(int instanceID, int line){
        var asset = EditorUtility.InstanceIDToObject(instanceID);
    }
}
