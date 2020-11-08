using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : BaseSingleton<GameManager> {
    bool isGMPanelShow = false;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (!isGMPanelShow) {
                print("show gm panel");
            } else {
                print("hide gm panel");
            }
        }
    }
}
