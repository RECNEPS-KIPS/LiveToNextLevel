using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopCanvas : MonoBehaviour {
    Transform fadePanel;
    Transform endPanel;
    Image fadeMask;
    Color fadePanelColor;
    Button againBtn;
    void Start() {
        fadePanel = transform.Find("FadeInOutMask").transform;
        fadeMask = fadePanel.GetComponent<Image>();
        fadePanelColor = fadeMask.color;
        endPanel = fadePanel.Find("EndPanel");
        FindObjectOfType<Player>().OnDeath += OnGameOver;//触发game over事件
        againBtn = Utils.FindObj<Button>(fadePanel, "AgainBtn");
        againBtn.onClick.AddListener(delegate () {
            PlayAgain();
        });
    }

    void Update() {

    }
    /// <summary>
    /// 弹出game over面板
    /// </summary>
    void OnGameOver() {
        //DontDestroyOnLoad(popCavas.gameObject);
        //print("Game Over");
        fadePanel.gameObject.SetActive(true);
        Color curColor = new Color(fadePanelColor.r, fadePanelColor.g, fadePanelColor.b, 0);
        Color tarColor = new Color(fadePanelColor.r, fadePanelColor.g, fadePanelColor.b, 1);
        StartCoroutine(Fade(curColor, tarColor, 1f));

    }
    /// <summary>
    /// 渐显协程
    /// </summary>
    /// <param name="cur">当前颜色</param>
    /// <param name="tar">目标颜色</param>
    /// <param name="time">渐显时间</param>
    /// <returns></returns>
    IEnumerator Fade(Color cur, Color tar, float time) {
        float speed = 1 / time;
        float percent = 0;
        while (percent < 1) {
            percent += Time.deltaTime * speed;
            fadeMask.color = Color.Lerp(cur, tar, percent);
            yield return null;
        }
    }
    /// <summary>
    /// play again的事件
    /// </summary>
    void PlayAgain() {
        //print("play again");
        SceneManager.LoadScene("Game");
        GameManager.Instance.isPlayerDeath = false;
        //fadePanel.gameObject.SetActive(false);
    }
}
