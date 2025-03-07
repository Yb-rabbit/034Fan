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
    private float flipDuration = 0.5f; // ��ת����ʱ��
    [SerializeField]
    private float gravity = -9.81f; // �������ٶ�

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����
    private Vector3 currentVelocity = Vector3.zero; // ��ǰ�ٶ�
    private Vector3 checkpoint; // ����λ��
    private AudioSource audioSource; // ��ƵԴ
    private bool isFlipping = false; // �Ƿ����ڷ�ת
    private Quaternion targetRotation; // Ŀ����ת
    private float flipStartTime; // ��ת��ʼʱ��
    private Vector3 moveDirection; // �ƶ�����
    private Vector3 velocity; // �ٶ�

    public event Action OnJump; // �����Ծ�¼�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // ȷ��������Ч
        rb.freezeRotation = true; // ������ת��ȷ��ֻ�ط����ƶ�

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
        // ��Ծ�߼�
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // ������λ��
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }

        // ����ת����
        HandleFlipInput();

        // ƽ����ת
        if (isFlipping)
        {
            float t = (Time.time - flipStartTime) / flipDuration;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            if (t >= 1f)
            {
                isFlipping = false;
                rb.useGravity = true; // ��ת������������������
            }
        }

        // ��ȡ����
        float horizontal = Input.GetAxis("Horizontal"); // A/D ��
        float vertical = Input.GetAxis("Vertical"); // W/S ��

        // �����ƶ�����
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Ӧ������
        if (!isGrounded && !isFlipping)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0; // ����ڵ����ϻ�ת�����У����ô�ֱ�ٶ�
        }
    }

    void FixedUpdate()
    {
        // ��ȡ�ƶ�����
        Vector3 targetVelocity = moveDirection;

        // �����Ƿ��ڵ����ϵ����ٶ�
        float targetSpeed = isGrounded ? groundSpeed : airSpeed;

        // �淶���ƶ����򣨱���Խ����ƶ��ٶȹ��죩
        if (targetVelocity != Vector3.zero)
        {
            targetVelocity.Normalize();
        }

        // ����Ŀ���ٶ�
        targetVelocity *= targetSpeed;

        // ʹ�� Lerp ʵ��ƽ�����ٶ�
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // �ƶ�����
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime + velocity * Time.fixedDeltaTime);
    }

    private void HandleFlipInput()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isFlipping) // ����W��ʱ��ת
        {
            StartFlip(90, 0, 0);
        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isFlipping) // ����S��ʱ��ת
        {
            StartFlip(-90, 0, 0);
        }
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !isFlipping) // ����A��ʱ��ת
        {
            StartFlip(0, 0, 90);
        }
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !isFlipping) // ����D��ʱ��ת
        {
            StartFlip(0, 0, -90);
        }
    }

    private void StartFlip(float xAngle, float yAngle, float zAngle)
    {
        isFlipping = true;
        flipStartTime = Time.time;
        targetRotation = transform.rotation * Quaternion.Euler(xAngle, yAngle, zAngle);
        rb.useGravity = false; // ��ת�����н�������
    }

    public bool Jump()
    {
        if (isGrounded)
        {
            // ������ϵ�����ʵ����Ծ
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // ��Ծ�����ڵ�����
            OnJump?.Invoke(); // ������Ծ�¼�
            return true; // ��Ծ�ɹ�
        }
        return false; // ��Ծʧ��
    }

    // ������
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
        if (collision.gameObject.CompareTag("Ground")) // �������ı�ǩ��"Ground"
        {
            isGrounded = false; // �뿪����ʱ����Ϊ���ڵ�����
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
        if (collision.gameObject.CompareTag("Ground")) // �������ı�ǩ��"Ground"
        {
            isGrounded = true; // ��������ʱ����Ϊ�ڵ�����
        }
    }

    // ���ü���λ��
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        checkpoint = newCheckpoint;
    }

    // ������һص�����
    private void Respawn()
    {
        transform.position = checkpoint;
        rb.velocity = Vector3.zero; // �����ٶ�

        // ����������Ч
        if (respawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(respawnSound);
        }
    }

    // ʵ��������ת���߼�
    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
