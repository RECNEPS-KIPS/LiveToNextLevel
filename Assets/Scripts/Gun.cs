using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public Transform fireTrs;//枪口位置
    public Bullet bullet;
    public float msBetweenShots = 100;//射击间隔
    public float fireSpeed = 35;//射击速度

    public Transform shell;//弹壳
    public Transform shellSpawner;//弹壳弹出点
    private float nextShotTime;
    public MuzzleFlash muzzleFlash;
    void Start() {
        muzzleFlash = GetComponent<MuzzleFlash>();
        bullet = Resources.Load<GameObject>("Prefabs/Bullet").GetComponent<Bullet>();
        shell = Resources.Load<GameObject>("Prefabs/Shell").transform;
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// 触发射击
    /// </summary>
    public void Shoot() {
        if (Time.time > nextShotTime) {
            nextShotTime = Time.time + msBetweenShots / 1000;
            Bullet newBullet = Instantiate(bullet, fireTrs.position, fireTrs.rotation) as Bullet;
            newBullet.SetSpeed(fireSpeed);

            Instantiate(shell.gameObject, shellSpawner.position, shellSpawner.rotation);
            muzzleFlash.Activate();
    }
    }
}