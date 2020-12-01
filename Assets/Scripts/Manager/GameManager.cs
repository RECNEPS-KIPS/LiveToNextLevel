// author:KIPKIPS
// time:2020.10.01 15:12:56
// describe:游戏管理者
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager> {
    bool isGMPanelShow = false;
    /// <summary>
    /// 开发者模式参数
    /// </summary>
    public bool DEVELOP_MODE = false;
    public bool gamePause = false;
    public bool isPlayerDeath = false;

    public override void Awake() {
        base.Awake();
    }
    private void Start() {
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (!isGMPanelShow) {
                //print("show gm panel");
                FindObjectOfType<PopCanvasManager>().OpenGMPanel();
            } else {
                //print("hide gm panel");
            }
            isGMPanelShow = !isGMPanelShow;
            gamePause = isGMPanelShow;
        }
    }
}
