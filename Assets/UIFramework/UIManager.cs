﻿// author:KIPKIPS
// time:2020.12.01 02:16:41
// describe:BasePanel UI面板管理类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseSingleton<UIManager> {
    //初始化的时候去解析面板的数据
    private Dictionary<int, PanelInfo> panelPathDict = new Dictionary<int, PanelInfo>();//存储UIpanel的面板和路径 字典
    private Dictionary<int, BasePanel> panelDict; //用来存储所有被实例化的panel上的BasePanel组件
    private Stack<BasePanel> panelStack;//存储显示出的ui界面的栈

    private Dictionary<string, int> panelNameIDMap;//panel的名称和ID的关系映射表
    public Transform canvasTrs;
    public override void Awake() {
        base.Awake();
        SavePanelInfoInDictByID();//保存数据
        canvasTrs = GameObject.Find("Canvas").transform;
    }
    // Start is called before the first frame update
    void Start() {
    }

    //用来解析面板的json数据
    public void SavePanelInfoInDictByID() {
        List<PanelInfo> list = DataManager.Instance.LoadDataByType<List<PanelInfo>>(DataManager.DataType.UIPanelData);
        if (panelNameIDMap == null) {
            panelNameIDMap = new Dictionary<string, int>();
        }
        for (var i = 0; i < list.Count; i++) {
            if (!panelPathDict.ContainsKey(list[i].ID)) {
                panelPathDict.Add(list[i].ID, list[i]);
            }
            if (!panelNameIDMap.ContainsKey(list[i].PanelName)) {
                panelNameIDMap.Add(list[i].PanelName, list[i].ID);
            }
        }
    }

    public int GetPanelID(string name) {
        //print(name);
        return panelNameIDMap[name];
    }
    public PanelInfo GetPanelInfo(int id) {
        //print(panelPathDict[id].PanelType);
        return panelPathDict[id];
    }

    //通过id获取panel
    public BasePanel GetPanel(int id) {
        if (panelDict == null) {
            panelDict = new Dictionary<int, BasePanel>();
        }
        BasePanel panel;
        panelDict.TryGetValue(id, out panel);
        if (panel != null) {
            return panel;
        } else {
            string path = panelPathDict[id].PanelPath;
            GameObject panelObj = Resources.Load<GameObject>(path);
            //print(panelObj.name);
            GameObject.Instantiate(panelObj, canvasTrs);//实例化

            panelDict.Add(id, panelObj.transform.GetComponent<BasePanel>());
            return panel;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    //把panel入栈,将当前显示的panel存储到栈顶
    public void PushPanelToStack(int panelID) {
        BasePanel panel = GetPanel(panelID);
        //构造初始化的空栈
        if (panelStack == null) {
            panelStack = new Stack<BasePanel>();
        }
        panelStack.Push(panel);
    }

    //把panel出栈,将当前显示的panel出栈,关闭
    public void PopPanelFromStack() {

    }
}
