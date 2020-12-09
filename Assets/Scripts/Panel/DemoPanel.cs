using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoPanel : BasePanel {
    public override void Awake() {
        base.Awake();
    }
    public override void Start() {
        base.Start();
    }
    public override void InitPanel() {
        base.InitPanel();
        Button testBtn = FindObj<Button>("Button", transform, delegate () {
            Debug.Log("On Test Button Click");
            UIManager.Instance.PopPanelFromStack();
        });
        //Button btn = FindObj<Button>("Button", transform);
    }
    public override void Update() {
        base.Update();
    }
    public override void OnEnter() {
        base.OnEnter();
    }

    //页面暂停
    public override void OnPause() {
        base.OnPause();
    }

    public override void OnResume() {
        base.OnResume();
    }

    public override void OnExit() {
        base.OnExit();
    }
}
