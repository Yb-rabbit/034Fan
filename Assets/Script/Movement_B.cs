using UnityEngine;
using UnityEngine.Audio;
using System;

public class Movement_B : MonoBehaviour
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

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����
    private Vector3 currentVelocity = Vector3.zero; // ��ǰ�ٶ�
    private Vector3 checkpoint; // ����λ��
    private AudioSource audioSource; // ��ƵԴ

    public event Action OnJump; // �����Ծ�¼�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // ȷ��������Ч

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
    }

    void FixedUpdate()
    {
        // ��ȡ�ƶ�����
        Vector3 targetVelocity = GetInputMovement();

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
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    private Vector3 GetInputMovement()
    {
        Vector3 movement = Vector3.zero;

        // ��Ӧ�ƶ�����
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //��ǰ
        {
            movement += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //����
        {
            movement += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //����
        {
            movement += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //����
        {
            movement += Vector3.right;
        }

        return movement;
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

    // ��ʼ������
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

    // ��ײ������״̬
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
        if (respawnSound != null)
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
