using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State Data", menuName = "State Data")]
public class StateMachineDataSO : ScriptableObject
{
    public List<StateHolder> stateList;
    public List<StateTransition> stateTransitionList;
    public int entryStateIndex;
}
[System.Serializable]
public class StateHolder
{
    public string name;
    public int index;
    public List<Type> behaviourTypeList;
#if UNITY_EDITOR
    public List<UnityEditor.MonoScript> scriptAssetList;
#endif

    public List<System.Object> CreateBehaviourInstance()
    {
        var res = new List<System.Object>();
        foreach (var behaviourType in this.behaviourTypeList)
        {
            var fullTypeName = behaviourType.FullName;
            var assembly = behaviourType.Assembly;
            var obj = assembly.CreateInstance(fullTypeName);
            res.Add(obj);
        }
        return res;
    }
#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    public void GenerateType()
    {
        this.behaviourTypeList = new List<Type>();
        foreach(var asset in scriptAssetList){
            this.behaviourTypeList.Add(asset.GetClass());
        }
    }
    [Sirenix.OdinInspector.Button]
    public void Test()
    {
        
    }
#endif
}
[Serializable]
public class StateTransition
{
    public int startIndex, endIndex;
}