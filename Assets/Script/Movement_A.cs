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

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����
    private Vector3 moveDirection; // �ƶ�����
    private AudioSource audioSource; // ��ƵԴ

    public event Action OnJump; // �����Ծ�¼�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // ȷ��������Ч
        rb.freezeRotation = false; // ������ת���Ա����
        rb.interpolation = RigidbodyInterpolation.Interpolate; // ���ò�ֵ

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

            // ʹ�� AddForce ʩ�������ƶ�����
            rb.AddForce(targetVelocity - rb.velocity, ForceMode.VelocityChange);

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
