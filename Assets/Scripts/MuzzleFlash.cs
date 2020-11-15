using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuzzleFlash : MonoBehaviour {
    public GameObject flashHolder;
    public Sprite[] flashSprites;
    public SpriteRenderer[] spriteRenderers;
    public float flashTime;

    private void Awake() {
        flashHolder = Utils.FindObj<Transform>(transform, "MuzzleFlash").gameObject;
    }
    private void Start() {
        Deactivate();
    }

    /// <summary>
    /// 激活枪口特效
    /// </summary>
    public void Activate() {
        flashHolder.SetActive(true);
        int flashSpriteIndex = Random.Range(0, flashSprites.Length);//随机一个索引
        for (int i = 0; i < spriteRenderers.Length; i++) {
            spriteRenderers[i].sprite = flashSprites[flashSpriteIndex];
        }
        Invoke("Deactivate", flashTime);
    }

    /// <summary>
    /// 取消激活枪口特效
    /// </summary>
    public void Deactivate() {
        flashHolder.SetActive(false);
    }
}
