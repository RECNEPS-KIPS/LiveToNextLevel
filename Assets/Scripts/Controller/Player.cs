﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity {
    public float moveSpeed = 5;
    public Camera viewCamera;
    private PlayerController playerController;
    private GunController gunController;
    public Transform crossSightTrs;//十字瞄具
    float gunHeight;//持有武器的高度
    public CrossSight crossSight;
    public Spawner spawner;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        crossSightTrs = Utils.FindObj<Transform>("CrossSight");
        crossSight = crossSightTrs.GetComponent<CrossSight>();
        viewCamera = Camera.main;
        gunHeight = gunController.GetGunHeight();

        spawner = FindObjectOfType<Spawner>();
        spawner.OnNewWave += OnNewWave;//订阅spawner开启下一波的事件
    }

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        //处理移动输入模块
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        playerController.Move(moveVelocity);

        //朝向处理模块
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);//相机指向鼠标的射线
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunHeight);
        float rayDis;
        if (groundPlane.Raycast(ray, out rayDis)) {
            Vector3 point = ray.GetPoint(rayDis);
            //Debug.DrawLine(ray.origin, point, Color.red);
            playerController.LookAt(point);
            crossSightTrs.position = new Vector3(point.x, gunHeight, point.z);
            crossSight.CheckTarget(ray);

            //计算瞄准点和玩家的距离,若太近则不瞄准
            if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude > 1f) {
                gunController.Aim(point);
            }

        }
        //武器处理模块
        if (Input.GetMouseButton(0)) {
            gunController.OnTriggerHolder();
        }
        if (Input.GetMouseButtonUp(0)) {
            gunController.OnTriggerRelease();
        }
        //装填弹药
        if (Input.GetKeyDown(KeyCode.R)) {
            gunController.ReloadClip();
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            gunController.SwitchFireMode();
        }
    }

    /// <summary>
    /// 处理spawner开启下一波时的逻辑
    /// </summary>
    /// <param name="waveNumber"></param>
    public void OnNewWave(int waveNumber) {
        HP = startHP;//下一波时恢复血量
        gunController.EquipGun(waveNumber - 1);
    }
}
