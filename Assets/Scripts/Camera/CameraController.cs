using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // 当前相机的Tranform
    public Transform CurrentCameraTrans;

    // 模拟当前水平和垂直输入
    public float HorizontalInput;
    public float VerticalInput;

    // 水平和垂直的移动速度
    public float HorizontalSpeed = 1;
    public float VerticalSpeed = 1;
    // 系数
    private float coefficient = 500;

    // 角度计算
    private float horizontalAngle;
    private float verticalAngle;

    // 垂直最大角度限制
    public float MaxVerticalAngle = 30;

    // 当前四元数
    private Quaternion currentQuaternion;

    void Update() {
        // 模拟玩家输入
        HorizontalInput = Input.GetAxis("Mouse X") * HorizontalSpeed * Time.deltaTime * coefficient;
        VerticalInput = Input.GetAxis("Mouse Y") * VerticalSpeed * Time.deltaTime * coefficient;

        // 计算水平和垂直的欧拉角度
        horizontalAngle += HorizontalInput;
        verticalAngle = Mathf.Clamp(verticalAngle - VerticalInput, -MaxVerticalAngle, MaxVerticalAngle);
        // 转换成四元数
        currentQuaternion = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
    }


    private void LateUpdate() {

        CurrentCameraTrans.rotation = currentQuaternion;

    }
}
