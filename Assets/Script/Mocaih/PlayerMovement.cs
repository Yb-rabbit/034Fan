using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // ��ͨ�ƶ��ٶ�
    public float dashSpeed = 15f; // �������ٶ�
    public float dashDuration = 0.5f; // ��̳���ʱ��
    public float dashCooldown = 1f; // �����ȴʱ��

    [Header("Reset Settings")]
    public float resetYValue = -10f; // ����Yֵ
    public Vector3 resetPosition = new Vector3(0, 1, 0); // ����λ��
    public AudioClip resetSound; // ������Ч
    public AudioMixerGroup audioMixerGroup; // ��������

    private float dashTimer = 0f; // ��̼�ʱ��
    private bool isDashing = false; // �Ƿ����ڳ��
    private Vector3 moveDirection; // �ƶ�����
    private Rigidbody rb; // Rigidbody ���
    private AudioSource audioSource; // ��ƵԴ

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // ��ȡ Rigidbody ���
        rb.useGravity = true; // ȷ��������Ӧ��
        rb.constraints = RigidbodyConstraints.FreezeRotation; // ������ת

        audioSource = GetComponent<AudioSource>(); // ��ȡ AudioSource ���
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // ���û�� AudioSource ����������һ��
        }

        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup; // ���û�������
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleDash();
        CheckResetCondition();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveY;

        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void HandleDash()
    {
        if (!isDashing && Time.time > dashTimer + dashCooldown)
        {
            if (Input.GetKey(KeyCode.Space) && moveDirection != Vector3.zero)
            {
                StartCoroutine(Dash(moveDirection));
                dashTimer = Time.time;
            }
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true; // ���Ϊ���ڳ��
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            float currentSpeed = Mathf.Lerp(moveSpeed, dashSpeed, elapsedTime / dashDuration);
            Vector3 targetPosition = rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        isDashing = false; // ��̽�����ȡ�����
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            HandleCollisionDuringDash();
        }
    }

    private void HandleCollisionDuringDash()
    {
        float reductionFactor = 0.5f; // ���ٱ���
        rb.velocity *= reductionFactor; // ����

        if (rb.velocity.magnitude < moveSpeed)
        {
            isDashing = false; // ����ٶȹ�С����ֹͣ���
        }
    }

    private void CheckResetCondition()
    {
        if (transform.position.y < resetYValue)
        {
            ResetPlayer();
        }
    }

    private void ResetPlayer()
    {
        transform.position = resetPosition; // ����λ��
        rb.velocity = Vector3.zero; // �����ٶ�

        if (resetSound != null)
        {
            audioSource.PlayOneShot(resetSound); // ����������Ч
        }

        // ���ó�ʼ����
        moveDirection = Vector3.zero;
        dashTimer = 0f;
        isDashing = false;
    }
}
