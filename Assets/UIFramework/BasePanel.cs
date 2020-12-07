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
    private int id;
    private UIPanelType panelType;
    private string path;
    public int ID {
        get { return id; }
        set { id = value; }
    }
    public UIPanelType PanelType {
        get { return panelType; }
        set { panelType = value; }
    }
    public string Path {
        get { return path; }
        set { path = value; }
    }
    public void Awake() {
        this.name = this.name.Replace("(Clone)", "");
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private CanvasGroup canvasGroup;
    void Start() {
        id = UIManager.Instance.GetPanelID(this.name);
        PanelInfo info = UIManager.Instance.GetPanelInfo(id);//得到面板的信息
        //print(info.PanelType);
        panelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), info.PanelType);//string转枚举
        path = info.PanelPath;
        InitPanel();
    }
    public virtual void InitPanel() {
    }
    // Update is called once per frame
    void Update() {

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

    }

    //暂停界面
    public virtual void OnPause() {
        canvasGroup.blocksRaycasts = false;//弹出新的面板时,鼠标和这个界面不再进行交互(禁用射线检测)
    }

    //恢复界面
    public virtual void OnResume() {

    }

    //关闭界面
    public virtual void OnExit() {

    }
}
