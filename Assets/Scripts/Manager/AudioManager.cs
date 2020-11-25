using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    float mainVolumePercent = 1;//主音量百分比
    float sfxVolumePercent = 1;//特技效果(Special Effects Cinematography)音量占比
    float musicVolumePercent = 1;//音乐音量占比

    //使用多个音频轨道,以便于在多个音轨上淡入淡出
    AudioSource[] musicSources;
    int activeMusicSourceIndex;//激活的音轨

    void Awake() {
        musicSources = new AudioSource[2];
        for (var i = 0; i < 2; i++) {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));//创建游戏对象
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }
    }

    //淡入淡出音轨
    public void PlayMusic(AudioClip clip, float fadeDuration = 1) {

    }

    //播放音频文件 在pos位置
    public void PlayAudioClip(AudioClip clip, Vector3 pos) {
        AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * mainVolumePercent);
    }
}
