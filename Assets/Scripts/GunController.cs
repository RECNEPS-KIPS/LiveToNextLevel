﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    public Gun equippedGun;
    public Gun defaultGun;
    public Transform weaponHolder;

    void Start() {
        weaponHolder = transform.Find("WeaponHolder");
        //Debug.Log(weaponHolder == null);
        defaultGun = Resources.Load<GameObject>("Prefabs/Gun").GetComponent<Gun>();
        if (defaultGun != null) {
            EquipGun(defaultGun);
        }
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

    public void Aim(Vector3 aimPoint) {
        if (equippedGun != null) {
            equippedGun.Aim(aimPoint);
        }
    }
}
