// author:KIPKIPS
// time:2020.11.25 22:31:18
// describe:音频管理模块
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;

public class AudioManager : BaseSingleton<AudioManager> {
    public enum AudioChannel {
        Main,//主音量
        SFX,//效果音
        Music,//音乐音量
    }
    public float mainVolumePercent = 1f;//主音量
    public float sfxVolumePercent = 0.2f;//2D音效音量占比
    public float musicVolumePercent = 1;//音乐音量占比

    //使用多个音频轨道,以便于在多个音轨上淡入淡出
    AudioSource[] musicSources;
    AudioSource sfxSource;

    Transform playerTrs;
    Transform audioListenerTrs;
    int activeMusicSourceIndex;//激活的音轨

    SoundLibrary library;

    new void Awake() {
        SetVolume(1f, AudioChannel.Main);
        SetVolume(0.2f, AudioChannel.SFX);
        SetVolume(1f, AudioChannel.Music);
        //加载保存的音量偏好数据
        AudioData data = DataManager.Instance.LoadDataByType<AudioData>(DataManager.DataType.Audio);
        if (data != null) {
            mainVolumePercent = data.mainVolumePercent;
            sfxVolumePercent = data.sfxVolumePercent;
            musicVolumePercent = data.musicVolumePercent;
        } else { //默认的音量方案
            mainVolumePercent = 1;
            sfxVolumePercent = 0.2f;
            musicVolumePercent = 1;
        }

        musicSources = new AudioSource[2];
        for (var i = 0; i < 2; i++) {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));//创建游戏对象
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }

        GameObject newSfx2DSource = new GameObject("SFX 2D Music Source ");//创建对象
        sfxSource = newSfx2DSource.AddComponent<AudioSource>();
        newSfx2DSource.transform.parent = transform;

        audioListenerTrs = FindObjectOfType<AudioListener>().transform;
        playerTrs = FindObjectOfType<Player>().transform;
        library = GetComponent<SoundLibrary>();
    }

    void Update() {
        if (playerTrs != null) {
            audioListenerTrs.position = playerTrs.position;
        }
    }

    //淡入淡出音轨
    public void PlayMusic(AudioClip clip, float fadeDuration = 1) {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;//始终得到 0 1 脉冲值
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(MusicFadeInOutPlay(fadeDuration));
    }
    //执行音轨播放淡入淡出的协程
    IEnumerator MusicFadeInOutPlay(float duration) {
        float percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime * 1 / duration;//时间微分值 * 1 / 持续时间
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, mainVolumePercent * mainVolumePercent, percent);//激活音轨淡入
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(mainVolumePercent * mainVolumePercent, 0, percent);//暂停音轨淡出
            yield return null;
        }
    }

    //播放音频文件 在pos位置
    public void PlayAudioClip(AudioClip clip, Vector3 pos) {
        if (clip != null) {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * mainVolumePercent);
        }
    }

    //根据音频名称播放
    public void PlayAudioClip(string soundName, Vector3 pos) {
        PlayAudioClip(library.GetClipFromName(soundName), pos);
    }

    //设置音量
    public void SetVolume(float volumePercent, AudioChannel channel) {
        switch (channel) {
            case AudioChannel.Main:
                mainVolumePercent = volumePercent;
                break;
            case AudioChannel.SFX:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
            default:
                mainVolumePercent = volumePercent;
                break;
        }
        //做数据的保存,存储玩家的音量偏好
        DataManager.Instance.SaveVolumeDataByChannel(channel, volumePercent);
    }

    //播放2D空间的音效,UI,关卡等音效
    public void PlayPlayAudioClip2D(string soundName) {
        sfxSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercent * mainVolumePercent);
    }
}
