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
    [SerializeField]
    private float flipDuration = 0.5f; // 翻转持续时间

    private Rigidbody rb; // 刚体
    private bool isGrounded = false; // 是否在地面上
    private Vector3 currentVelocity = Vector3.zero; // 当前速度
    private AudioSource audioSource; // 音频源
    private bool isFlipping = false; // 是否正在翻转
    private Quaternion targetRotation; // 目标旋转
    private float flipStartTime; // 翻转开始时间
    private Vector3 moveDirection; // 移动方向
    private bool isFalling = false; // 是否正在下落

    public event Action OnJump; // 添加跳跃事件

    void Start()
    {
        InitializeRigidbody();
        InitializeCollider();
        InitializeAudioSource();
        InitializeCheckpoint();
        InitializePhysicsSettings();
    }

    void Update()
    {
        HandleJumpInput();
        HandleRespawn();
        HandleMovementInput();

        if (isFlipping)
        {
            UpdateFlip();
        }
        else
        {
            UpdateFlipDirection();
        }

        CheckFalling();
    }

    void FixedUpdate()
    {
        if (isGrounded || !isFalling)
        {
            MoveCharacter();
        }
    }

    private void InitializeRigidbody()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.freezeRotation = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.maxAngularVelocity = 7.0f;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
    }

    private void InitializeCollider()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null && rollingMaterial != null)
        {
            collider.material = rollingMaterial;
        }
    }

    private void InitializeAudioSource()
    {
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }
    }

    private void InitializeCheckpoint()
    {
        if (checkpoint == Vector3.zero)
        {
            checkpoint = transform.position;
        }
    }

    private void InitializePhysicsSettings()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.defaultContactOffset = 0.01f;
        Physics.defaultSolverIterations = 6;
        Physics.defaultSolverVelocityIterations = 1;
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
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    private void MoveCharacter()
    {
        if (moveDirection != Vector3.zero)
        {
            float targetSpeed = isGrounded ? groundSpeed : airSpeed;
            Vector3 targetVelocity = moveDirection * targetSpeed;
            Vector3 smoothedVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            currentVelocity = smoothedVelocity;
            rb.MovePosition(rb.position + smoothedVelocity * Time.fixedDeltaTime);
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
            isFalling = false;
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
            isFalling = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void UpdateFlipDirection()
    {
        if (moveDirection != Vector3.zero && isGrounded)
        {
            Vector3 axis = Vector3.Cross(Vector3.up, moveDirection);
            float angle = Vector3.SignedAngle(transform.forward, moveDirection, Vector3.up);
            Quaternion newRotation = Quaternion.AngleAxis(angle, axis) * transform.rotation;
            StartFlip(newRotation);
        }
    }

    private void UpdateFlip()
    {
        float t = (Time.time - flipStartTime) / flipDuration;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);

        if (t >= 1f)
        {
            isFlipping = false;
            rb.freezeRotation = false;
        }
    }

    private void CheckFalling()
    {
        if (rb.velocity.y < 0 && !isFalling)
        {
            isFalling = true;
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

    public void StartFlip(Quaternion newRotation)
    {
        if (!isFlipping)
        {
            isFlipping = true;
            targetRotation = newRotation;
            flipStartTime = Time.time;
            rb.freezeRotation = true;
        }
    }
}

