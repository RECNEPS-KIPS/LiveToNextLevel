using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoPanel : BasePanel {
    public override void InitPanel() {
        Button testBtn = FindObj<Button>("Button", transform, delegate () {
            Debug.Log("On Test Button Click");
        });
        //Button btn = FindObj<Button>("Button", transform);
        //print(testBtn == null);
    }
}
