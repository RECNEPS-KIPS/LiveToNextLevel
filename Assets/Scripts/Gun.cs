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
    public int clipCount;//弹夹子弹容量
    public int clipRemainBulletCount;//当前弹夹剩余子弹数量
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
    public float reloadTime = 3;
    bool isClipReloading;//是否正在重新装填弹药
    void Start() {
        //初始化
        clipRemainBulletCount = clipCount;
        isClipReloading = false;
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

        //未处于装填状态且弹夹为空
        if (!isClipReloading && clipRemainBulletCount == 0) {
            ReloadClip();
        }
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
        //没有在装填,时间上满足射击间隔,并且弹夹剩余子弹不为0
        if (!isClipReloading && Time.time > nextShotTime && clipRemainBulletCount > 0) {
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
                if (clipRemainBulletCount == 0) {
                    break;
                }
                clipRemainBulletCount--;
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
    /// 装填弹药
    /// </summary>
    public void ReloadClip() {
        StartCoroutine(Reload());
    }
    IEnumerator Reload() {
        isClipReloading = true;
        yield return new WaitForSeconds(0.5f);//装填时间
        float percent = 0;
        float reloadSpeed = 1f / reloadTime;
        Vector3 oriAngle = transform.localEulerAngles;
        float maxReloadAngle = 30;//最大角度30°;
        while (percent < 1) {
            percent += Time.deltaTime * reloadSpeed;
            float deltaAngle = (-(percent * percent) + percent) * 4;
            float angle = Mathf.Lerp(0, maxReloadAngle, deltaAngle);
            transform.localEulerAngles = oriAngle + Vector3.left * angle;

            yield return null;
        }

        //装填完毕
        isClipReloading = false;
        clipRemainBulletCount = clipCount;
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