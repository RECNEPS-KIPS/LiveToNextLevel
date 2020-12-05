// author:KIPKIPS
// time:2020.12.01 02:16:41
// describe:BasePanel UI面板管理类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseSingleton<UIManager> {
    //初始化的时候去解析面板的数据
    private Dictionary<int, PanelInfo> panelPathDict = new Dictionary<int, PanelInfo>();//存储UIpanel的面板和路径 字典
    private Dictionary<int, BasePanel> panelDict; //用来存储所有被实例化的panel上的BasePanel组件
    public override void Awake() {
        base.Awake();
        SavePanelInfoInDictByID();//保存数据
    }
    // Start is called before the first frame update
    void Start() {

    }
    public BasePanel GetPanelByID(int id) {
        if (panelDict == null) {
            panelDict = new Dictionary<int, BasePanel>();
        }
        BasePanel panel = panelDict.TryGet(id);
        if (panel != null) {
            return panel;
        } else {
            string path = panelPathDict[id].PanelPath;
            panel = Resources.Load<BasePanel>(path);
            panelDict.Add(id, panel);
            return panel;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    //用来解析面板的json数据
    public void SavePanelInfoInDictByID() {
        List<PanelInfo> list = DataManager.Instance.LoadDataByType<List<PanelInfo>>(DataManager.DataType.UIPanelType);
        for (var i = 0; i < list.Count; i++) {
            if (!panelPathDict.ContainsKey(list[i].ID)) {
                panelPathDict.Add(list[i].ID, list[i]);
            }
        }
    }
}
