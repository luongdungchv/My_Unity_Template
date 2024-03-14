using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Codice.ThemeImages;
using System.Text.RegularExpressions;
using System.IO;

public class SceneHelperEditor : OdinEditorWindow
{
    [MenuItem("Window/Scene Helper")]
    public static void ShowWindow(){
        var window = GetWindow<SceneHelperEditor>();
        window.titleContent = new GUIContent("Scene Helper");
        window.Show();
    }

    //[OnValueChanged("SceneAssetListChange")]
    public List<SceneAsset> sceneAssetList;

    private void SceneAssetListChange(){
        for(int i = 0; i < sceneAssetList.Count; i++){
            var asset = sceneAssetList[i];
            var assetPath = AssetDatabase.GetAssetPath(asset);

            var menuItemMatch = "//menu item placeholder";
            var scenePathMatch = "scene name placeholder";

            var regexMenuItem = menuItemMatch + i;
            var regexScenePath = scenePathMatch + i;
            var fileContent = File.ReadAllText("Assets/Editor/ShortMenu.cs");

            if(Regex.Match(fileContent, regexMenuItem).Length == 0){
                regexMenuItem = $".+?//{i}";
            }
            if(Regex.Match(fileContent, regexScenePath).Length == 0){
                regexScenePath = $"{assetPath}";
            }

            string modified = Regex.Replace(fileContent, regexMenuItem, $"[MenuItem(\"Open Scenes/{asset.name}\")]//{i}");
            var final = Regex.Replace(modified, regexScenePath, $"{assetPath}");

            File.WriteAllText("Assets/Editor/ShortMenu.cs", final);
        }
    }
    [Button]
    public void Test(){
        SceneAssetListChange();
        AssetDatabase.Refresh();
    }
}
