// author:KIPKIPS
// time:2020.11.26 23:32:11
// describe:游戏数据保存管理类,负责保存玩家的数据
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : BaseSingleton<DataManager> {
    public enum DataType {
        Audio,
        UIPanelData,
    }

    //用来保存音量数据
    public void SaveVolumeDataByChannel(AudioManager.AudioChannel channel, float value) {
        AudioData data = LoadDataByType<AudioData>(DataType.Audio);//需要覆盖旧的数据,所以这里加载存在的json文件
        if (data == null) {
            data = new AudioData();//第一次加载则创建新的对象
        }
        //print(data);
        //保存数据
        switch (channel) {
            case AudioManager.AudioChannel.Main:
                data.mainVolumePercent = value;
                break;
            case AudioManager.AudioChannel.SFX:
                data.sfxVolumePercent = value;
                break;
            case AudioManager.AudioChannel.Music:
                data.musicVolumePercent = value;
                break;
            default:
                data.mainVolumePercent = value;
                break;
        }
        string filePath = Application.dataPath + "/GameData" + "/VolumeData.json";
        //将对象转化为字符串
        string jsonStr = JsonConvert.SerializeObject(data);
        //print(jsonStr);
        //将转换过后的json字符串写入json文件
        StreamWriter writer = new StreamWriter(filePath);
        writer.Write(jsonStr);//写入文件
        writer.Close();
        AssetDatabase.Refresh();
    }

    //在json文件中存储UIPanel面板的数据
    public void SaveUIPanelInfo(PanelInfo info) {
        List<PanelInfo> list = LoadDataByType<List<PanelInfo>>(DataType.UIPanelData);//需要覆盖旧的数据,所以这里加载存在的json文件
        if (list == null) {
            list = new List<PanelInfo>();
        }
        string filePath = Application.dataPath + "/GameData" + "/UIPanelData.json";
        //将对象转化为字符串

        if (list.Count == 0) {
            list.Add(info);
        } else {
            for (var i = 0; i < list.Count; i++) {
                if (list[i].PanelName == info.PanelName) {
                    list[i].PanelName = info.PanelName;
                    list[i].PanelType = info.PanelType;
                    list[i].PanelPath = info.PanelPath;
                    break;
                    //print("here");
                } else {
                    if (i == list.Count - 1) {
                        list.Add(info);
                        //print("there");
                    }
                }
            }

        }

        string jsonStr = JsonConvert.SerializeObject(list);
        //print(jsonStr);
        //将转换过后的json字符串写入json文件
        StreamWriter writer = new StreamWriter(filePath);
        writer.Write(jsonStr);//写入文件
        writer.Close();
        AssetDatabase.Refresh();
    }

    //通过json文件名加载json文件
    public T JSONLoadByName<T>(string fileName) {
        string filePath = Application.dataPath + "/GameData" + "/" + fileName + ".json";
        //print(filePath);
        //读取文件
        StreamReader reader = new StreamReader(filePath);
        string jsonStr = reader.ReadToEnd();
        reader.Close();
        //Debug.Log(jsonStr);
        //字符串转换为DataSave对象
        T data = JsonConvert.DeserializeObject<T>(jsonStr);
        return data;
    }

    //通过数据type加载json数据
    public T LoadDataByType<T>(DataType type) {
        switch (type) {
            case DataType.Audio:
                return JSONLoadByName<T>("VolumeData");
            case DataType.UIPanelData:
                return JSONLoadByName<T>("UIPanelData");
            default:
                return JSONLoadByName<T>("VolumeData");
        }

    }
}
