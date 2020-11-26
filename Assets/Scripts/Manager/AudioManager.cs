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
        AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * mainVolumePercent);
    }
}
