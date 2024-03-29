﻿// author:KIPKIPS
// time:2020.12.05 01:43:52
// describe:BasePanel UI面板的基类
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BasePanel : MonoBehaviour {
    // Start is called before the first frame update
    private int id;
    private UIPanelType panelType;
    private string path;
    public int ID {
        get {
            return id;
        }
        set {
            id = value;
        }
    }
    public UIPanelType PanelType {
        get {
            return panelType;
        }
        set {
            panelType = value;
        }
    }
    public string Path {
        get { return path; }
        set { path = value; }
    }
    private CanvasGroup canvasGroup;

    public CanvasGroup CanvasGroup {
        get {
            if (canvasGroup == null) {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            return canvasGroup;
        }
    }
    public float fadeInOutTime;//渐隐渐显time
    public float scaleTime;//缩放time
    public bool showTween = false;
    public virtual void Awake() {
        this.name = this.name.Replace("(Clone)", "");
        fadeInOutTime = 0.3f;
        scaleTime = 0.3f;
    }
    public virtual void Start() {
        id = UIManager.Instance.GetPanelID(this.name);
        PanelInfo info = UIManager.Instance.GetPanelInfo(id);//得到面板的信息
        //print(info.PanelType);
        panelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), info.PanelType);//string转枚举
        path = info.PanelPath;
        InitPanel();
    }
    public virtual void InitPanel() {
        if (showTween) {
            transform.localScale = Vector3.zero;
            CanvasGroup.alpha = 0;

        } else {
            transform.localScale = Vector3.one;
            CanvasGroup.alpha = 1;
        }
    }
    // Update is called once per frame
    public virtual void Update() {

    }

    //FindObj接口列表
    public T FindObj<T>(string name) {
        return Utils.FindObj<T>(this.transform, name);
    }
    public T FindObj<T>(string name, Transform trs) {
        Transform root = trs == null ? this.transform : trs;
        return Utils.FindObj<T>(this.transform, name);
    }
    public T FindObj<T>(string name, Transform trs, Action func) {
        Transform root = trs == null ? this.transform : trs;
        T resObj = Utils.FindObj<T>(trs, name);
        if (typeof(T) == typeof(Button)) { //button支持绑定函数方法
            (resObj as Button).onClick.AddListener(delegate () {
                func();
            });
        }
        return resObj;
    }

    //界面生命周期流程,这里只提供虚方法,具体的逻辑由各个业务界面进行重写

    //进入界面
    public virtual void OnEnter() {
        if (showTween) {
            transform.DOScale(Vector3.one, scaleTime).SetEase(Ease.InOutBack).OnComplete(() => transform.localScale = Vector3.one);
            CanvasGroup.DOFade(1, fadeInOutTime).SetEase(Ease.InOutBack).OnComplete(() => CanvasGroup.alpha = 1);
        }
        CanvasGroup.blocksRaycasts = true;
    }

    //暂停界面
    public virtual void OnPause() {
        CanvasGroup.blocksRaycasts = false;//弹出新的面板时,鼠标和这个界面不再进行交互(禁用射线检测)
    }

    //恢复界面
    public virtual void OnResume() {
        CanvasGroup.blocksRaycasts = true;
    }

    //关闭界面
    public virtual void OnExit() {
        if (showTween) {
            CanvasGroup.DOFade(0, fadeInOutTime).SetEase(Ease.InOutBack).OnComplete(() => CanvasGroup.alpha = 0);
            transform.DOScale(Vector3.zero, scaleTime).SetEase(Ease.InOutBack).OnComplete(() => transform.localScale = Vector3.zero);
        }
        CanvasGroup.blocksRaycasts = false;
    }
}
