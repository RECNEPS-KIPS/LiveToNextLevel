using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    //开火模式
    public enum FireMode {
        Auto, //自动
        Burst, //点射
        Single,//单点
    }
    public int burstCount;//点射子弹数量
    public int shotsRemainInBurst;
    public FireMode fireMode;
    public Transform[] fireTrs;//枪口位置
    public Bullet bullet;
    public float msBetweenShots = 100;//射击间隔
    public float fireSpeed = 35;//射击速度

    public Transform shell;//弹壳
    public Transform shellSpawner;//弹壳弹出点
    private float nextShotTime;
    public MuzzleFlash muzzleFlash;
    public Vector3 recoilSmoothDampVelocity;
    public float recoilRotSmoothDampVelocity;
    public float recoilAngle;
    [Header("后坐力相关变量")]
    public Vector2 kickTrsMinMax = new Vector2(0.05f, 0.2f);//后坐力位移范围值
    public Vector2 kickAngleMinMax = new Vector2(3, 5);//后坐力旋转范围值
    public float recoilTrsRecoverTime = 0.1f;//后坐力位移恢复时间
    public float recoilAngleRecoverTime = 0.1f;//后坐力旋转恢复时间
    bool triggerRelease;//标识扳机是否释放
    void Start() {
        muzzleFlash = GetComponent<MuzzleFlash>();
        bullet = Resources.Load<GameObject>("Prefabs/Bullet").GetComponent<Bullet>();
        shell = Resources.Load<GameObject>("Prefabs/Shell").transform;

        shotsRemainInBurst = burstCount;
    }

    // Update is called once per frame
    void LateUpdate() {
        //模拟后坐力回弹
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilTrsRecoverTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilAngleRecoverTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;
    }

    /// <summary>
    /// 瞄准
    /// </summary>
    /// <param name="aimPoint">瞄准目标点</param>
    public void Aim(Vector3 aimPoint) {
        transform.LookAt(aimPoint);
    }

    /// <summary>
    /// 触发射击
    /// </summary>
    private void Shoot() {
        if (Time.time > nextShotTime) {
            if (fireMode == FireMode.Burst) {
                if (shotsRemainInBurst == 0) {
                    return;
                }
                shotsRemainInBurst--;
            } else if (fireMode == FireMode.Single) {
                if (!triggerRelease) {
                    return;
                }
            }
            for (int i = 0; i < fireTrs.Length; i++) {
                nextShotTime = Time.time + msBetweenShots / 1000;
                Bullet newBullet = Instantiate(bullet, fireTrs[i].position, fireTrs[i].rotation) as Bullet;
                newBullet.SetSpeed(fireSpeed);
            }
            Instantiate(shell.gameObject, shellSpawner.position, shellSpawner.rotation);
            muzzleFlash.Activate();
            transform.localPosition -= Vector3.forward * Random.Range(kickTrsMinMax.x, kickTrsMinMax.y);//射击时位置后移,模拟后坐力
            recoilAngle += Random.Range(kickAngleMinMax.x, kickAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
        }
    }

    /// <summary>
    /// 扳机按下
    /// </summary>
    public void OnTriggerHolder() {
        Shoot();
        triggerRelease = false;
    }
    /// <summary>
    /// 扳机释放
    /// </summary>
    public void OnTriggerRelease() {
        triggerRelease = true;
        shotsRemainInBurst = burstCount;
    }
}