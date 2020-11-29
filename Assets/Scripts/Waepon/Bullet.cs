using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Bullet : MonoBehaviour {
    public Color trailColor;
    public float speed = 6f;
    public LayerMask collisionMask;//检测碰撞的layer
    public float damage = 2;
    private float existTime = 3.0f;//在场景保存的时间

    private float skinWidth = 0;
    void Start() {
        Destroy(gameObject, existTime);
        //GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
        GetComponent<TrailRenderer>().startColor = trailColor;
        GetComponent<TrailRenderer>().endColor = Color.white;
    }
    /// <summary>
    /// 设置子弹速度
    /// </summary>
    /// <param name="newSpeed">速度大小</param>
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
        Collider[] nearColliders = Physics.OverlapSphere(transform.position, 0.2f, collisionMask);//检测附近有无敌人
        if (nearColliders.Length > 0) {
            OnHitObject(nearColliders[0], transform.position);//damage 第一个(最近的敌人)
        }
    }
    // Update is called once per frame
    void Update() {
        float moveDistance = Time.deltaTime * speed;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
        // TODO:子弹的y轴坐标固定为水平的,第三人称 
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
    }

    //检测碰撞
    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        //与trigger的碰撞也要触发,加上QueryTriggerInteraction.Collide
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit.collider, hit.point);
        }
    }

    //碰撞触发
    void OnHitObject(RaycastHit hit) {
        //print(hit.transform.gameObject.name);
        IDamageable damageObject = hit.collider.GetComponent<IDamageable>();
        if (damageObject != null) {
            //print("hit");s
            damageObject.TakeDamage(damage);
        }
        GameObject.Destroy(this.gameObject);
    }

    //重写碰撞触发方法
    void OnHitObject(Collider collider, Vector3 hitPoint) {
        IDamageable damageableObject = collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
        GameObject.Destroy(gameObject);
    }
}
