﻿using System.Collections;
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
    public PopCanvasManager popCanvasManager;

    private void Start() {
        popCanvasManager = FindObjectOfType<PopCanvasManager>();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (!isGMPanelShow) {
                //print("show gm panel");
                popCanvasManager.OpenGMPanel();
            } else {
                //print("hide gm panel");
            }
            isGMPanelShow = !isGMPanelShow;
            gamePause = isGMPanelShow;
        }
    }
}