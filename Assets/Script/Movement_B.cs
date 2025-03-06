using UnityEngine;
using UnityEngine.Audio;
using System;

public class Movement_B : MonoBehaviour
{
    [SerializeField]
    private float groundSpeed = 3f; // 地面上的移速
    [SerializeField]
    private float airSpeed = 1.5f; // 空中的移速
    [SerializeField]
    private float jumpForce = 10f; // 跳跃力
    [SerializeField]
    private float acceleration = 5f; // 加速度
    [SerializeField]
    private float fallThreshold = -10f; // y 轴低于该值时传送回检查点
    [SerializeField]
    private LayerMask groundLayer; // 地面层
    [SerializeField]
    private AudioClip respawnSound; // 重生音效
    [SerializeField]
    private AudioMixerGroup audioMixerGroup; // 音频混音器组

    private Rigidbody rb; // 刚体
    private bool isGrounded = false; // 是否在地面上
    private Vector3 currentVelocity = Vector3.zero; // 当前速度
    private Vector3 checkpoint; // 检查点位置
    private AudioSource audioSource; // 音频源

    public event Action OnJump; // 添加跳跃事件

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // 确保重力生效

        // 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 设置音频混音器组
        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        // 初始地面检测
        CheckGroundStatus();

        // 初始化检查点为起始位置
        checkpoint = transform.position;
    }

    void Update()
    {
        // 跳跃逻辑
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // 检查玩家位置
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void FixedUpdate()
    {
        // 获取移动方向
        Vector3 targetVelocity = GetInputMovement();

        // 根据是否在地面上调整速度
        float targetSpeed = isGrounded ? groundSpeed : airSpeed;

        // 规范化移动方向（避免对角线移动速度过快）
        if (targetVelocity != Vector3.zero)
        {
            targetVelocity.Normalize();
        }

        // 计算目标速度
        targetVelocity *= targetSpeed;

        // 使用 Lerp 实现平滑加速度
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // 移动物体
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    private Vector3 GetInputMovement()
    {
        Vector3 movement = Vector3.zero;

        // 响应移动输入
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //往前
        {
            movement += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //往后
        {
            movement += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //往左
        {
            movement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //往右
        {
            movement += Vector3.right;
        }

        return movement;
    }

    public bool Jump()
    {
        if (isGrounded)
        {
            // 添加向上的力来实现跳跃
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // 跳跃后不再在地面上
            OnJump?.Invoke(); // 触发跳跃事件
            return true; // 跳跃成功
        }
        return false; // 跳跃失败
    }

    // 地面检测
    void OnCollisionEnter(Collision collision)
    {
        CheckGroundStatus(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        CheckGroundStatus(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = false; // 离开地面时设置为不在地面上
        }
    }

    // 初始地面检测
    private void CheckGroundStatus()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f, groundLayer))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
    }

    // 碰撞检测地面状态
    private void CheckGroundStatus(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = true; // 碰到地面时设置为在地面上
        }
    }

    // 设置检查点位置
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        checkpoint = newCheckpoint;
    }

    // 传送玩家回到检查点
    private void Respawn()
    {
        transform.position = checkpoint;
        rb.velocity = Vector3.zero; // 重置速度

        // 播放重生音效
        if (respawnSound != null)
        {
            audioSource.PlayOneShot(respawnSound);
        }
    }

    // 实现重置旋转的逻辑
    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
