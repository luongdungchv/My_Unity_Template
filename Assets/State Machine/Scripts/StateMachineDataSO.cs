using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State Data", menuName = "State Data")]
public class StateMachineDataSO : ScriptableObject
{
    public List<StateHolder> stateList;
    public List<StateTransition> stateTransitionList;
}
[System.Serializable]
public class StateHolder{
    public string name;
    public int index;
    public Type behaviourType;
    #if UNITY_EDITOR
    public UnityEditor.MonoScript scriptAsset;
    #endif

    public System.Object CreateBehaviourInstance(){
        var fullTypeName = behaviourType.FullName;
        var assembly = behaviourType.Assembly;
        var res = assembly.CreateInstance(fullTypeName);
        return res;
    }
}
[Serializable]
public class StateTransition{
    public int startIndex, endIndex;
}