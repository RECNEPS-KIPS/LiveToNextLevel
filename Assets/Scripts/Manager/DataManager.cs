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
    }

    //用来保存音量数据
    public void SaveVolumeDataByChannel(AudioManager.AudioChannel channel, float value) {
        DataSave data = JSONLoadByName("VolumeData");//需要覆盖旧的数据,所以这里加载存在的json文件

        //保存数据
        switch (channel) {
            case AudioManager.AudioChannel.Main:
                data.volumeData.mainVolumePercent = value;
                break;
            case AudioManager.AudioChannel.SFX:
                data.volumeData.sfxVolumePercent = value;
                break;
            case AudioManager.AudioChannel.Music:
                data.volumeData.musicVolumePercent = value;
                break;
            default:
                data.volumeData.mainVolumePercent = value;
                break;
        }

        string filePath = Application.dataPath + "/GameData" + "/VolumeData.json";
        //将对象转化为字符串
        string jsonStr = JsonConvert.SerializeObject(data);
        Console.WriteLine(jsonStr);
        //将转换过后的json字符串写入json文件
        StreamWriter writer = new StreamWriter(filePath);
        writer.Write(jsonStr);//写入文件
        writer.Close();
        AssetDatabase.Refresh();
    }
    public DataSave JSONLoadByName(string fileName) {
        string filePath = Application.dataPath + "/GameData" + "/" + fileName + ".json";
        //读取文件
        StreamReader reader = new StreamReader(filePath);
        string jsonStr = reader.ReadToEnd();
        reader.Close();
        //Debug.Log(jsonStr);
        //字符串转换为save对象
        DataSave data = JsonConvert.DeserializeObject<DataSave>(jsonStr);
        return data;
    }
    public DataSave LoadDataByType(DataType type) {
        switch (type) {
            case DataType.Audio:
                return JSONLoadByName("VolumeData");
            default:
                return JSONLoadByName("VolumeData");
        }

    }
}
