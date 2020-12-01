using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

//编辑器作用对象
[CustomEditor(typeof(MapGenerator))]
public class BuildPanelToJsonData : Editor {
    [MenuItem("Tools/BulidPanel", false, 3)]
    static void BulidPanelToJsonData() {
        //Debug.Log("BulidPanelToJsonData...");
        EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", 0);//进度条

        if (Selection.gameObjects.Length < 1) {
            Debug.LogError("Select Nothing");
            return;
        }
        foreach (GameObject item in Selection.gameObjects) {
            string path = AssetDatabase.GetAssetPath(item);
            if (!path.EndsWith(".prefab"))
                return;
            //Debug.Log(path);
            string pattern = @"Resources/[\S]+/?";
            string relativePath = "";
            foreach (Match match in Regex.Matches(path, pattern)) {
                relativePath = match.ToString().Replace("Resources/", "").Replace(".prefab", "").ToString();//获取相对路径
            }
            int id = GetCurrentAssignableID();
            PanelInfo panelInfo = new PanelInfo(relativePath, item.name, id);
            DataManager.Instance.SaveUIPanelInfo(panelInfo);
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    static int GetCurrentAssignableID() {
        List<PanelInfo> list = DataManager.Instance.LoadDataByType<List<PanelInfo>>(DataManager.DataType.UIPanelType);
        if (list == null) {
            return 1;
        } else {
            return list.Count + 1;
        }

    }
}

