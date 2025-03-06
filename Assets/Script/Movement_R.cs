using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_R : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f; // 旋转速度因子

    [SerializeField]
    private float smoothTime = 0.1f; // 平滑时间因子

    private float targetRotationY;
    private float currentRotationY;
    private float rotationVelocity;

    // Update 在每一帧调用一次
    void Update()
    {
        // 获取鼠标滚轮输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // 根据滚动方向设置目标旋转角度
        if (scrollInput != 0)
        {
            targetRotationY += scrollInput * rotationSpeed;
        }

        // 平滑插值当前旋转角度到目标旋转角度
        currentRotationY = Mathf.SmoothDamp(currentRotationY, targetRotationY, ref rotationVelocity, smoothTime);

        // 应用旋转
        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }

    // 重置旋转属性
    public void ResetRotation()
    {
        targetRotationY = 0;
        currentRotationY = 0;
        rotationVelocity = 0;
        transform.rotation = Quaternion.identity;
    }
}
