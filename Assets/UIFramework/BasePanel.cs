// author:KIPKIPS
// time:2020.12.05 01:43:52
// describe:BasePanel UI面板的基类
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour {
    // Start is called before the first frame update
    public int ID;
    public UIPanelType panelType;
    public string path;
    public void Awake() {
    }
    void Start() {
        // ID = UIManager.Instance.GetPanelID(this.name);
        // PanelInfo info = UIManager.Instance.GetPanelInfo(ID);//得到面板的信息
        // panelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), info.PanelType);//string转枚举
        // path = info.PanelPath;
        InitPanel();
    }
    public virtual void InitPanel() {

    }
    // Update is called once per frame
    void Update() {

    }

    public virtual void OnShowPanel() {
        UIManager.Instance.PushPanelToStack(this.ID);
    }
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
}
