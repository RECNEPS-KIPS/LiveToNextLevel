﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CrossSight : MonoBehaviour {
    Transform dot;
    public LayerMask targetLayer;//检测标记层
    public SpriteRenderer dotSpriteRenderer;
    public Color dotOriColor;//中心点的原始颜色
    public Color dotHeightColor;//中心点高光颜色
    void Start() {
        Cursor.visible = false;//将鼠标指针隐藏掉
        dot = Utils.FindObj<Transform>(transform, "Dot");
        dotSpriteRenderer = dot.GetComponent<SpriteRenderer>();
        dotSpriteRenderer.color = dotOriColor;
    }

    void Update() {
        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
    }

    /// <summary>
    /// 检测目标点
    /// </summary>
    /// <param name="ray">射线</param>
    public void CheckTarget(Ray ray) {
        if (Physics.Raycast(ray, 100, targetLayer)) {//检测到目标中心点高亮
            dotSpriteRenderer.color = dotHeightColor;
        } else {
            dotSpriteRenderer.color = dotOriColor;//将颜色重置为默认的颜色值
        }

    }
}
