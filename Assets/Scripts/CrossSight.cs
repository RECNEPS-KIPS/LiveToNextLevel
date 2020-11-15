using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CrossSight : MonoBehaviour {
    Transform dot;
    public LayerMask targetLayer;//检测标记层
    public SpriteRenderer dotSpriteRenderer;
    public Color dotOriColor;//中心点的原始颜色
    public Color dotHeightColor;//中心点高光颜色
    void Start() {
        Cursor.visible = false;
        dot = Utils.FindObj<Transform>(transform, "Dot");
        dotSpriteRenderer = dot.GetComponent<SpriteRenderer>();
        dotSpriteRenderer.color = dotOriColor;
    }

    void Update() {
        transform.Rotate(Vector3.forward * 40 * Time.deltaTime);
    }
    public void CheckTarget(Ray ray) {
        if (Physics.Raycast(ray, 100, targetLayer)) {//检测到目标中心点高亮
            dotSpriteRenderer.color = dotHeightColor;
        } else {
            dotSpriteRenderer.color = dotOriColor;
        }
        
    }
}
