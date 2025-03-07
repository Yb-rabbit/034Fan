using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_R : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f; // 旋转速度因子

    [SerializeField]
    private float jumpForce = 5f; // 跳跃力

    [SerializeField]
    private float smoothTime = 0.1f; // 平滑时间因子

    private float targetRotationX;
    private float currentRotationX;
    private float rotationVelocity;
    private bool isJumping; // 是否正在跳跃
    private bool isPreparingJump; // 是否正在准备跳跃

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentRotationX = 0f;
        isJumping = false;
        isPreparingJump = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update 在每一帧调用一次
    void Update()
    {
        // 检测按下R键
        if (Input.GetKeyDown(KeyCode.R) && !isJumping && !isPreparingJump)
        {
            isPreparingJump = true;
            targetRotationX = 45f; // 目标旋转角度
        }

        // 平滑旋转到目标角度
        if (isPreparingJump)
        {
            currentRotationX = Mathf.SmoothDamp(currentRotationX, targetRotationX, ref rotationVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(currentRotationX, 0, 0);

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
            currentRotationX = Mathf.SmoothDamp(currentRotationX, targetRotationX, ref rotationVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(currentRotationX, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测是否着地
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            targetRotationX = 0;
            currentRotationX = 0;
            rotationVelocity = 0;
            transform.rotation = Quaternion.identity;
        }
    }

    // 重置旋转属性
    public void ResetRotation()
    {
        targetRotationX = 0;
        currentRotationX = 0;
        rotationVelocity = 0;
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
}

