using UnityEngine;
using UnityEngine.Audio;
using System;

public class Movement_A : MonoBehaviour
{
    [SerializeField]
    private float groundSpeed = 3f; // �����ϵ�����
    [SerializeField]
    private float airSpeed = 1.5f; // ���е�����
    [SerializeField]
    private float jumpForce = 10f; // ��Ծ��
    [SerializeField]
    private float acceleration = 5f; // ���ٶ�
    [SerializeField]
    private float fallThreshold = -10f; // y ����ڸ�ֵʱ���ͻؼ���
    [SerializeField]
    private LayerMask groundLayer; // �����
    [SerializeField]
    private AudioClip respawnSound; // ������Ч
    [SerializeField]
    private AudioMixerGroup audioMixerGroup; // ��Ƶ��������
    [SerializeField]
    private float rollSpeed = 10f; // �����ٶ�
    [SerializeField]
    private PhysicMaterial rollingMaterial; // �������
    [SerializeField]
    private Vector3 checkpoint; // ����λ��
    [SerializeField]
    private float flipDuration = 0.5f; // ��ת����ʱ��

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����
    private Vector3 currentVelocity = Vector3.zero; // ��ǰ�ٶ�
    private AudioSource audioSource; // ��ƵԴ
    private bool isFlipping = false; // �Ƿ����ڷ�ת
    private Quaternion targetRotation; // Ŀ����ת
    private float flipStartTime; // ��ת��ʼʱ��
    private Vector3 moveDirection; // �ƶ�����
    private bool isFalling = false; // �Ƿ���������

    private float printInterval = 1.0f; // ��ӡ���ʱ�䣨�룩
    private float lastPrintTime; // �ϴδ�ӡʱ��

    public event Action OnJump; // �����Ծ�¼�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // ȷ��������Ч
        rb.freezeRotation = false; // ������ת���Ա����
        rb.interpolation = RigidbodyInterpolation.Interpolate; // ���ò�ֵ
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // ������ײ���
        rb.maxAngularVelocity = 7.0f; // ���������ٶ�

        // �����������
        Collider collider = GetComponent<Collider>();
        if (collider != null && rollingMaterial != null)
        {
            collider.material = rollingMaterial;
        }

        // ���ø�������
        rb.drag = 0.5f; // ��������
        rb.angularDrag = 0.5f; // ������

        // ��ȡ AudioSource ���
        if (!TryGetComponent(out audioSource))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ������Ƶ��������
        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        // ��ʼ������Ϊ��ʼλ��
        if (checkpoint == Vector3.zero)
        {
            checkpoint = transform.position;
        }

        // ����ȫ������
        Physics.gravity = new Vector3(0, -9.81f, 0);
        Physics.defaultContactOffset = 0.01f; // ����Ĭ�ϽӴ�ƫ��
        Physics.defaultSolverIterations = 6; // ����Ĭ���������������
        Physics.defaultSolverVelocityIterations = 1; // ����Ĭ��������ٶȵ�������

        lastPrintTime = Time.time; // ��ʼ���ϴδ�ӡʱ��
    }

    void Update()
    {
        HandleJumpInput();
        HandleRespawn();
        HandleMovementInput();

        if (isFlipping)
        {
            float t = (Time.time - flipStartTime) / flipDuration;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);

            if (t >= 1f)
            {
                isFlipping = false;
                rb.freezeRotation = false; // ����������ת
            }
        }
        else
        {
            UpdateFlipDirection();
        }

        // ����Ƿ�ʼ����
        if (rb.velocity.y < 0 && !isFalling)
        {
            isFalling = true;
        }
    }

    void FixedUpdate()
    {
        if (isGrounded || !isFalling)
        {
            MoveCharacter();
        }
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
        float horizontal = Input.GetAxis("Horizontal"); // A/D ��
        float vertical = Input.GetAxis("Vertical"); // W/S ��
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;
    }

    private void MoveCharacter()
    {
        if (moveDirection != Vector3.zero)
        {
            float targetSpeed = isGrounded ? groundSpeed : airSpeed;
            Vector3 targetVelocity = moveDirection * targetSpeed;

            // ʹ�� Vector3.Lerp ƽ���ٶȱ仯
            Vector3 smoothedVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            currentVelocity = smoothedVelocity;
            rb.MovePosition(rb.position + smoothedVelocity * Time.fixedDeltaTime);

            // ʹ�� AddTorque ʩ����ת��������������
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
            isFalling = false; // ��������״̬
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
            isFalling = false; // ��������״̬
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
            float angle = Vector3.SignedAngle(transform.forward, moveDirection, Vector3.up); // ʹ�� SignedAngle ����Ƕ�
            Quaternion newRotation = Quaternion.AngleAxis(angle, axis) * transform.rotation;
            StartFlip(newRotation);
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
            rb.freezeRotation = true; // ������ת
        }
    }
}

