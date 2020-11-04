using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    Transform endPanel;
    Image fadeMask;
    void Start() {
        endPanel = GameObject.Find("FadeInOutMask").transform;
        fadeMask = endPanel.GetComponent<Image>();
        FindObjectOfType<Player>().OnDeath += OnGameOver;//触发game over事件
    }

    void Update() {

    }
    /// <summary>
    /// 弹出game over面板
    /// </summary>
    void OnGameOver() {

        endPanel.gameObject.SetActive(true);

    }
}
