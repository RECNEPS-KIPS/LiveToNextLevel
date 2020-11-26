// author:KIPKIPS
// time:2020.11.26 23:32:11
// describe:音频资源管理模块
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioClip mainTheme;
    public AudioClip menuTheme;
    private void Awake() {
        mainTheme = Resources.Load<AudioClip>("Audio/Music/MainTheme");
        menuTheme = Resources.Load<AudioClip>("Audio/Music/MenuTheme");
    }
    private void Start() {
        AudioManager.Instance.PlayMusic(menuTheme, 2);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AudioManager.Instance.PlayMusic(mainTheme, 3);
        }
    }
}
