using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections;

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
    private float flipDuration = 0.5f; // ��ת����ʱ��
    [SerializeField]
    private float rollSpeed = 10f; // �����ٶ�
    [SerializeField]
    private PhysicMaterial rollingMaterial; // �������

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����
    private Vector3 currentVelocity = Vector3.zero; // ��ǰ�ٶ�
    private Vector3 checkpoint; // ����λ��
    private AudioSource audioSource; // ��ƵԴ
    private bool isFlipping = false; // �Ƿ����ڷ�ת
    private Quaternion targetRotation; // Ŀ����ת
    private float flipStartTime; // ��ת��ʼʱ��
    private Vector3 moveDirection; // �ƶ�����

    public event Action OnJump; // �����Ծ�¼�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // ȷ��������Ч
        rb.freezeRotation = false; // ������ת���Ա����

        // �����������
        Collider collider = GetComponent<Collider>();
        if (collider != null && rollingMaterial != null)
        {
            collider.material = rollingMaterial;
        }

        // ��ȡ AudioSource ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ������Ƶ��������
        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup;
        }

        // ��ʼ������
        CheckGroundStatus();

        // ��ʼ������Ϊ��ʼλ��
        checkpoint = transform.position;
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
            rb.AddForce(targetVelocity * acceleration, ForceMode.Acceleration);

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
        CheckGroundStatus(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        CheckGroundStatus(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void CheckGroundStatus(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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

