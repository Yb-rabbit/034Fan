using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_R : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 120f; // 旋转速度因子

    [SerializeField]
    private float jumpForce = 15f; // 跳跃力

    [SerializeField]
    private float smoothTime = 0.1f; // 平滑时间因子

    private float targetRotationX;
    private float currentRotationX;
    private bool isJumping; // 是否正在跳跃
    private bool isPreparingJump; // 是否正在准备跳跃
    private float initialRotationX; // 初始旋转角度
    private Quaternion initialRotation; // 初始旋转

    private Rigidbody rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentRotationX = 0f;
        isJumping = false;
        isPreparingJump = false;
        mainCamera = Camera.main;
    }

    // Update 在每一帧调用一次
    void Update()
    {
        // 检测按下R键
        if (Input.GetKeyDown(KeyCode.R) && !isJumping && !isPreparingJump)
        {
            PrepareJump();
        }

        // 平滑旋转到目标角度
        if (isPreparingJump)
        {
            currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, Time.deltaTime / smoothTime);
            transform.rotation = Quaternion.Euler(currentRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            // 检查是否已经旋转到目标角度
            if (Mathf.Abs(currentRotationX - targetRotationX) < 0.1f)
            {
                isPreparingJump = false;
                isJumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        // 旋转立方体
        if (isJumping)
        {
            targetRotationX += rotationSpeed * Time.deltaTime;
            if (targetRotationX - initialRotationX >= 360f)
            {
                targetRotationX = initialRotationX + 360f; // 限制旋转角度为一圈
                isJumping = false; // 停止旋转
            }
            currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, Time.deltaTime / smoothTime);
            transform.rotation = initialRotation * Quaternion.Euler(currentRotationX, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测是否着地
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            ResetRotation(); // 重置旋转状态
        }
    }

    // 重置旋转属性
    public void ResetRotation()
    {
        targetRotationX = 0;
        currentRotationX = 0;
        isJumping = false;
        isPreparingJump = false;
        transform.rotation = Quaternion.identity;
    }

    // 回到检查点时取消所有附加属性
    public void ResetToCheckpoint()
    {
        ResetRotation();
        rb.velocity = Vector3.zero; // 重置速度
        rb.angularVelocity = Vector3.zero; // 重置角速度
        transform.position = Vector3.zero; // 重置位置
    }

    // 检查是否在运动
    public bool IsMoving()
    {
        return isJumping || isPreparingJump;
    }

    // 矫正朝向为摄像机的朝向
    private void CorrectOrientation()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; // 忽略Y轴的影响
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    // 公共方法，用于外部调用以触发起跳功能
    public void PrepareJump()
    {
        if (!isJumping && !isPreparingJump)
        {
            isPreparingJump = true;
            targetRotationX = 45f; // 目标旋转角度
            initialRotationX = currentRotationX; // 记录初始旋转角度
            initialRotation = transform.rotation; // 记录初始旋转
            CorrectOrientation(); // 矫正朝向
        }
    }
}
