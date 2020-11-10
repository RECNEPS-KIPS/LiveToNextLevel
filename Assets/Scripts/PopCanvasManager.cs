using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopCanvasManager : MonoBehaviour { //BaseSingleton<PopCanvasManager>
    Transform fadePanel;
    Transform endPanel;
    Image fadeMask;
    Color fadePanelColor;
    Button againBtn;
    Transform popMessageTrs;
    void Start() {
        popMessageTrs = Utils.FindObj<Transform>(transform, "PopMessageTrs");
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
        print("game over");
        Color curColor = new Color(fadePanelColor.r, fadePanelColor.g, fadePanelColor.b, 0);
        Color tarColor = new Color(fadePanelColor.r, fadePanelColor.g, fadePanelColor.b, 1);
        StartCoroutine(GameOverPanelFade(curColor, tarColor, 1f));

    }
    /// <summary>
    /// 渐显协程
    /// </summary>
    /// <param name="cur">当前颜色</param>
    /// <param name="tar">目标颜色</param>
    /// <param name="time">渐显时间</param>
    /// <returns></returns>
    IEnumerator GameOverPanelFade(Color cur, Color tar, float time) {
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

        fadePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 上浮弹出框
    /// </summary>
    /// <param name="pos">弹出框的位置</param>
    /// <param name="msg">弹出框展示的文本</param>
    /// <param name="color">文本色号</param>
    public void PopMessage(Vector3 pos, string msg, Color color) {
        GameObject pop = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/PopMessagePanel"), popMessageTrs);
        //print(popMessageTrs == null);
        pop.transform.localPosition = pos;
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        string addColorMsg = "<color=#" + hexColor + ">" + msg + "</color>";
        Text txt = Utils.FindObj<Text>(pop.transform, "Text");
        txt.text = addColorMsg;

        //fade out
        CanvasGroup canvas = Utils.FindObj<CanvasGroup>(pop.transform, null);
        StartCoroutine(PopMessagePanelFadeAndPop(pop, canvas, 1f, 50));
    }
    IEnumerator PopMessagePanelFadeAndPop(GameObject pop, CanvasGroup canvas, float fadeTime, float moveSpeed) {
        float alphaPercent = 1;
        float y = 0;
        Destroy(pop, fadeTime + 0.5f);
        float fadeSpeed = 1 / fadeTime;
        while (alphaPercent > 0) {
            y += Time.deltaTime * fadeSpeed * moveSpeed;
            pop.transform.localPosition = new Vector3(0, y, 0);
            alphaPercent -= Time.deltaTime * fadeSpeed;
            canvas.alpha = Mathf.Lerp(0, 1, alphaPercent);
            yield return null;
        }
    }
}
