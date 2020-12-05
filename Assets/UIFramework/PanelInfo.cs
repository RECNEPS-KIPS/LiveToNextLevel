using UnityEngine;

[System.Serializable]
public class PanelInfo {
    private string panelPath;//面板存储的路径
    private string panelType;//面板类型
    private string panelName;//面板名称
    private int id;//面板的唯一标识
    public string PanelPath {
        get {
            return panelPath;
        }
        set {
            panelPath = value;
        }
    }
    public string PanelType {
        get {
            return panelType;
        }
        set {
            panelType = value;
        }
    }
    public string PanelName {
        get {
            return panelName;
        }
        set {
            panelName = value;
        }
    }
    public int ID {
        get {
            return id;
        }
        set {
            id = value;
        }
    }
    public PanelInfo(string path, string name, int id) {
        this.PanelPath = path;
        this.PanelName = name;
        this.ID = id;
    }
}