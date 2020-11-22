using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {
    public Rigidbody rigid;
    public float forceMin = 90;
    public float forceMax = 120;
    float liveTime = 2f;//暂留时间
    void Start() {
        rigid = GetComponent<Rigidbody>();
        float force = Random.Range(forceMin, forceMax);//随机一个力,介于min,max之间
        rigid.AddForce(transform.right * force);
        rigid.AddTorque(Random.insideUnitSphere * force);
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// 弹壳渐隐协程
    /// </summary>
    /// <returns></returns>
    IEnumerator Fade() {
        yield return new WaitForSeconds(liveTime);//等待暂存时间后继续执行下面的操作
        float percent = 0;
        float fadeSpeed = 3;
        Material mat = GetComponent<Renderer>().material;
        Color startColor = mat.color;
        Color tarColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (percent < 1) {
            percent += Time.deltaTime * fadeSpeed;
            //print(percent);
            mat.color = Color.Lerp(startColor, tarColor, percent);
            yield return null;
        }
        Destroy(gameObject);
    }
}
