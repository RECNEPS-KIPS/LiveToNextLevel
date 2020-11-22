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
    public Text newWaveTitle;
    public Text newWaveCount;
    Spawner spawner;
    public RectTransform newWaveBanner;
    private void Awake() {
        spawner = FindObjectOfType<Spawner>();
        spawner.OnNewWave += OnNewWave;
    }
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
        newWaveBanner = Utils.FindObj<RectTransform>(transform, "WaveTipsBanner");
        newWaveTitle = Utils.FindObj<RectTransform>(newWaveBanner.transform, "WaveText").GetComponent<Text>();
        newWaveCount = Utils.FindObj<RectTransform>(newWaveBanner.transform, "EnemyCountText").GetComponent<Text>();
    }

    /// <summary>
    /// 下一波敌人的UI触发事件
    /// </summary>
    /// <param name="waveNumber"></param>
    void OnNewWave(int waveNumber) {
        newWaveTitle.text = "- 第" + waveNumber + "波敌人来袭 - ";
        string str = "";
        if (spawner.waves[waveNumber - 1].enemyCount < 0) {
            str = "无限";
        }
        else {
            str = spawner.waves[waveNumber - 1].enemyCount + "";
        }
        newWaveCount.text = "数量:" + str;
        StopCoroutine("WaveTipsBanner");
        StartCoroutine("WaveTipsBanner");
    }

    /// <summary>
    /// 波数提示Banner
    /// </summary>
    /// <returns></returns>
    IEnumerator WaveTipsBanner() {
        float delayTime = 1.5f;//顶部停留时间
        float animePercent = 0;
        float speed = 3f;
        int dir = 1;
        float endDelayTime = Time.time + 1 / speed + delayTime;
        while (animePercent >= 0) {
            animePercent += Time.deltaTime * speed * dir;
            if (animePercent >= 1) {
                animePercent = 1;
                if (Time.time > endDelayTime) {
                    dir = -1;
                }
            }
            newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-130, 200, animePercent);
            yield return null;
        }
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
    /// 处理game over面板渐显
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
    /// <summary>
    /// popTips渐隐上浮协程
    /// </summary>
    /// <param name="pop"></param>
    /// <param name="canvas"></param>
    /// <param name="fadeTime"></param>
    /// <param name="moveSpeed"></param>
    /// <returns></returns>
    IEnumerator PopMessagePanelFadeAndPop(GameObject pop, CanvasGroup canvas, float fadeTime, float moveSpeed) {
        float percent = 0;
        float y = 0;
        Destroy(pop, fadeTime + 0.5f);
        float fadeSpeed = 1 / fadeTime;
        while (percent < 1) {
            y += Time.deltaTime * fadeSpeed * moveSpeed;
            pop.transform.localPosition = new Vector3(0, y, 0);
            percent += Time.deltaTime * fadeSpeed;
            canvas.alpha = Mathf.Lerp(1, 0, percent);
            yield return null;
        }
    }

    public void OpenGMPanel() {
        Transform gmPanel = Utils.FindObj<Transform>(transform, "GM");
        gmPanel.localScale = Vector3.zero;
        gmPanel.gameObject.SetActive(true);
        Utils.FindObj<Button>(gmPanel.transform, "ModeBtn").onClick.AddListener(delegate () {
            SetToggleState();
        });
        StartCoroutine(GMPanelScaleAnimate(gmPanel, 0.5f));
    }
    IEnumerator GMPanelScaleAnimate(Transform gm, float gmScaleTime) {
        float percent = 0;
        float speed = 1 / gmScaleTime;
        float y = 0;
        Cursor.visible = true;
        //Invoke("SetTimeScale", gmScaleTime);
        while (percent < 1) {
            percent += Time.deltaTime * speed;
            y = (-5 / 2.0f) * percent * percent + (7 / 2.0f) * percent;
            // print(y);
            gm.localScale = new Vector3(y, y, 1);
            yield return null;
        }
    }
    void SetTimeScale() {
        Time.timeScale = 0;
    }
    public void SetToggleState() {
        // print("?s??");
    }
}
