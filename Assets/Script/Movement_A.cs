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
    private float rollSpeed = 10f; // 滚动速度
    [SerializeField]
    private PhysicMaterial rollingMaterial; // 物理材质
    [SerializeField]
    private Vector3 checkpoint; // 检查点位置

    private Rigidbody rb; // 刚体
    private bool isGrounded = false; // 是否在地面上
    private Vector3 moveDirection; // 移动方向
    private AudioSource audioSource; // 音频源

    public event Action OnJump; // 添加跳跃事件

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // 确保重力生效
        rb.freezeRotation = false; // 允许旋转，以便滚动
        rb.interpolation = RigidbodyInterpolation.Interpolate; // 启用插值

        // 设置物理材质
        Collider collider = GetComponent<Collider>();
        if (collider != null && rollingMaterial != null)
        {
            collider.material = rollingMaterial;
        }

        // 设置刚体阻尼
        rb.drag = 0.5f; // 线性阻尼
        rb.angularDrag = 0.5f; // 角阻尼

        // 获取 AudioSource 组件
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 设置音频混音器组
        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        // 初始化检查点为起始位置
        if (checkpoint == Vector3.zero)
        {
            checkpoint = transform.position;
        }
    }

    void Update()
    {
        HandleJumpInput();
        HandleRespawn();
        HandleMovementInput();
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void HandleRespawn()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D 键
        float vertical = Input.GetAxis("Vertical"); // W/S 键
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    private void MoveCharacter()
    {
        if (moveDirection != Vector3.zero)
        {
            float targetSpeed = isGrounded ? groundSpeed : airSpeed;
            Vector3 targetVelocity = moveDirection * targetSpeed;

            // 使用 AddForce 施加力来移动方块
            rb.AddForce(targetVelocity - rb.velocity, ForceMode.VelocityChange);

            // 使用 AddTorque 施加旋转力矩来滚动方块
            Vector3 torque = Vector3.Cross(Vector3.up, moveDirection) * rollSpeed;
            rb.AddTorque(torque, ForceMode.Acceleration);
        }
    }

    public bool Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            OnJump?.Invoke();
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Respawn()
    {
        transform.position = checkpoint;
        rb.velocity = Vector3.zero;

        if (respawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(respawnSound);
        }
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        checkpoint = newCheckpoint;
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
