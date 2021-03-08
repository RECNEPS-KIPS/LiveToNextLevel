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
    public enum GunType {
        Pistol = 1,//手枪
        Musketry = 2,//步枪
        Submachine = 3,//冲锋枪
    }
    public GunType gunType;
    private int curFireModeIndex = 1;
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
    private float nextShotTime;//下一次射击的时间
    public MuzzleFlash muzzleFlash;//管理枪口的特效
    public Vector3 recoilSmoothDampVelocity;
    public float recoilRotSmoothDampVelocity;
    public float recoilAngle;
    [Header("后坐力相关变量")]
    public Vector2 kickTrsMinMax = new Vector2(0.05f, 0.2f);//后坐力位移范围值
    public Vector2 kickAngleMinMax = new Vector2(3, 5);//后坐力旋转范围值
    public float recoilTrsRecoverTime = 0.1f;//后坐力位移恢复时间
    public float recoilAngleRecoverTime = 0.1f;//后坐力旋转恢复时间
    bool triggerRelease;//标识扳机是否释放
    public float reloadTime = 0.1f;//装填时间
    bool isClipReloading;//是否正在重新装填弹药

    //处理音频的变量
    public AudioClip shootAudio;
    public AudioClip reloadAudio;
    void Start() {
        //初始化
        clipRemainBulletCount = clipCount;
        isClipReloading = false;
        muzzleFlash = GetComponent<MuzzleFlash>();
        bullet = Resources.Load<GameObject>("Prefabs/Common/Bullet").GetComponent<Bullet>();
        shell = Resources.Load<GameObject>("Prefabs/Common/Shell").transform;

        shootAudio = Resources.Load<AudioClip>("Audio/Guns/GunShoot_" + (int)gunType);
        reloadAudio = Resources.Load<AudioClip>("Audio/Guns/GunReload_" + (int)gunType);

        //fireMode = FireMode.Auto;
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
        if (!isClipReloading) {
            transform.LookAt(aimPoint);
        }
    }

    /// <summary>
    /// 触发射击
    /// </summary>
    private void Shoot() {
        //没有在装填,时间上满足射击间隔,并且弹夹剩余子弹不为0
        if (!isClipReloading && Time.time > nextShotTime && clipRemainBulletCount > 0) {
            curFireModeIndex = 1;
            if (fireMode == FireMode.Burst) {
                curFireModeIndex = 2;
                if (shotsRemainInBurst == 0) {
                    return;
                }
                shotsRemainInBurst--;
            } else if (fireMode == FireMode.Single) {
                curFireModeIndex = 3;
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

            AudioManager.Instance.PlayAudioClip(shootAudio, transform.position);//射击音效
        }
    }

    /// <summary>
    /// 装填弹药
    /// </summary>
    public void ReloadClip() {
        //未处于装填状态且弹夹不是满的
        if (!isClipReloading && clipRemainBulletCount != clipCount) {
            StartCoroutine(Reload());
            AudioManager.Instance.PlayAudioClip(reloadAudio, transform.position);
        }

    }
    IEnumerator Reload() {
        isClipReloading = true;
        yield return new WaitForSeconds(0.2f);//装填时间
        float percent = 0;
        float reloadSpeed = 1f / reloadTime;
        Vector3 oriAngle = transform.localEulerAngles;
        float maxReloadAngle = 20;//最大角度30°;
        while (percent < 1) {
            percent += Time.deltaTime * reloadSpeed;
            float deltaAngle = (-Mathf.Pow(percent, 2) + percent) * 4;
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

    /// <summary>
    /// 切换开火模式
    /// </summary>
    public void SwitchFireMode() {
        string str = "";
        int nextModeIndex = curFireModeIndex + 1 > 3 ? curFireModeIndex - 2 : curFireModeIndex + 1;
        curFireModeIndex = nextModeIndex;
        //print(curFireModeIndex);
        fireMode = NumberToFireMode(curFireModeIndex);
        if (isClipReloading) {
            str = "装填弹药中...";
        } else {
            str = FireModeToName(fireMode);
        }
        //print(str);
        FindObjectOfType<PopCanvasManager>().PopMessage(Vector3.zero, "切换到" + str + "模式", Color.white);
    }
    /// <summary>
    /// 开火模式转中文名称
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public string FireModeToName(FireMode mode) {
        string res = "";
        switch (mode) {
            case FireMode.Auto: res = "自动"; break;
            case FireMode.Burst: res = "点射"; break;
            case FireMode.Single: res = "单点"; break;
            default: res = "自动"; break;
        }
        return res;
    }
    /// <summary>
    /// 武器模式索引转开火模式枚举类型
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public FireMode NumberToFireMode(int index) {
        FireMode mode = FireMode.Auto;
        switch (index) {
            case 1: mode = FireMode.Auto; break;
            case 2: mode = FireMode.Burst; break;
            case 3: mode = FireMode.Single; break;
            default: mode = FireMode.Auto; break;
        }
        return mode;
    }
}