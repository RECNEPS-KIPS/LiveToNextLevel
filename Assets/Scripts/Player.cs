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
    public Transform crossSight;//十字瞄具

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        playerController = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        crossSight = Utils.FindObj<Transform>("CrossSight");
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        //处理移动输入模块
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        playerController.Move(moveVelocity);

        //朝向处理模块
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);//相机指向鼠标的射线
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDis;
        if (groundPlane.Raycast(ray, out rayDis)) {
            Vector3 point = ray.GetPoint(rayDis);
            //Debug.DrawLine(ray.origin, point, Color.red);
            playerController.LookAt(point);
            crossSight.position = new Vector3(point.x, gunController.GetGunHeight(), point.z);
        }
        //武器处理模块
        if (Input.GetMouseButton(0)) {
            gunController.OnTriggerHolder();
        }
        if (Input.GetMouseButtonUp(0)) {
            gunController.OnTriggerRelease();
        }
    }
}
