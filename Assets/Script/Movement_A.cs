using UnityEngine;
using UnityEngine.Audio;
using System;

public class Movement_A : MonoBehaviour
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
    [SerializeField]
    private float flipDuration = 0.5f; // 翻转持续时间
    [SerializeField]
    private float gravity = -9.81f; // 重力加速度

    private Rigidbody rb; // 刚体
    private bool isGrounded = false; // 是否在地面上
    private Vector3 currentVelocity = Vector3.zero; // 当前速度
    private Vector3 checkpoint; // 检查点位置
    private AudioSource audioSource; // 音频源
    private bool isFlipping = false; // 是否正在翻转
    private Quaternion targetRotation; // 目标旋转
    private float flipStartTime; // 翻转开始时间
    private Vector3 moveDirection; // 移动方向
    private Vector3 velocity; // 速度

    public event Action OnJump; // 添加跳跃事件

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // 确保重力生效
        rb.freezeRotation = true; // 禁用旋转，确保只沿方向移动

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

        // 处理翻转输入
        HandleFlipInput();

        // 平滑翻转
        if (isFlipping)
        {
            float t = (Time.time - flipStartTime) / flipDuration;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            if (t >= 1f)
            {
                isFlipping = false;
                rb.useGravity = true; // 翻转结束后重新启用重力
            }
        }

        // 获取输入
        float horizontal = Input.GetAxis("Horizontal"); // A/D 键
        float vertical = Input.GetAxis("Vertical"); // W/S 键

        // 计算移动方向
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // 应用重力
        if (!isGrounded && !isFlipping)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0; // 如果在地面上或翻转过程中，重置垂直速度
        }
    }

    void FixedUpdate()
    {
        // 获取移动方向
        Vector3 targetVelocity = moveDirection;

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
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime + velocity * Time.fixedDeltaTime);
    }

    private void HandleFlipInput()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isFlipping) // 按下W键时翻转
        {
            StartFlip(90, 0, 0);
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isFlipping) // 按下S键时翻转
        {
            StartFlip(-90, 0, 0);
        }
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isFlipping) // 按下A键时翻转
        {
            StartFlip(0, 0, 90);
        }
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isFlipping) // 按下D键时翻转
        {
            StartFlip(0, 0, -90);
        }
    }

    private void StartFlip(float xAngle, float yAngle, float zAngle)
    {
        isFlipping = true;
        flipStartTime = Time.time;
        targetRotation = transform.rotation * Quaternion.Euler(xAngle, yAngle, zAngle);
        rb.useGravity = false; // 翻转过程中禁用重力
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
        if (respawnSound != null && audioSource != null)
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
