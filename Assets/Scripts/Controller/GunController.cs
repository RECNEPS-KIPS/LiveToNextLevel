﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    public Gun equippedGun;
    public Gun[] allGuns;
    public Transform weaponHolder;

    public void Awake() {
        weaponHolder = transform.Find("WeaponHolder");
    }
    void Start() {
        //Debug.Log(weaponHolder == null);
        // defaultGun = Resources.Load<GameObject>("Prefabs/Weapon/Gun/Gun2").GetComponent<Gun>();
        // if (defaultGun != null) {
        //     EquipGun(defaultGun);
        // }
    }
    /// <summary>
    /// 装备武器
    /// </summary>
    /// <param name="gunToEquip">需要装备的武器</param>
    public void EquipGun(Gun gunToEquip) {
        //当前角色身上装备有武器,先把旧的武器销毁掉
        if (equippedGun != null) {
            Destroy(equippedGun.gameObject);
        }
        //实例化武器预制体
        equippedGun = Instantiate(gunToEquip, weaponHolder.position, weaponHolder.rotation) as Gun;
        equippedGun.transform.parent = weaponHolder;
    }

    /// <summary>
    /// 重写装备函数
    /// </summary>
    /// <param name="gunIndex">装备的武器索引</param>
    public void EquipGun(int gunIndex) {
        EquipGun(allGuns[gunIndex]);
    }

    /// <summary>
    /// 扣下扳机
    /// </summary>
    public void OnTriggerHolder() {
        if (equippedGun != null) {
            equippedGun.OnTriggerHolder();
        }
    }
    /// <summary>
    /// 释放扳机
    /// </summary>
    public void OnTriggerRelease() {
        if (equippedGun != null) {
            equippedGun.OnTriggerRelease();
        }
    }
    /// <summary>
    /// 获取枪的高度
    /// </summary>
    /// <returns></returns>
    public float GetGunHeight() {
        if (equippedGun != null) {
            return equippedGun.transform.position.y;
        } else {
            return -1;
        }
    }

    /// <summary>
    /// 瞄准
    /// </summary>
    /// <param name="aimPoint"></param>
    public void Aim(Vector3 aimPoint) {
        if (equippedGun != null) {
            equippedGun.Aim(aimPoint);
        }
    }

    /// <summary>
    /// 装填弹药
    /// </summary>
    public void ReloadClip() {
        if (equippedGun != null) {
            equippedGun.ReloadClip();
        }
    }

    public void SwitchFireMode() {
        if (equippedGun != null) {
            equippedGun.SwitchFireMode();
        }
    }

}
