using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // 普通移动速度
    public float dashSpeed = 15f; // 冲刺最大速度
    public float dashDuration = 0.5f; // 冲刺持续时间
    public float dashCooldown = 1f; // 冲刺冷却时间

    [Header("Reset Settings")]
    public float resetYValue = -10f; // 重置Y值
    public Vector3 resetPosition = new Vector3(0, 1, 0); // 重置位置
    public AudioClip resetSound; // 重置音效
    public AudioMixerGroup audioMixerGroup; // 混音器组

    private float dashTimer = 0f; // 冲刺计时器
    private bool isDashing = false; // 是否正在冲刺
    private Vector3 moveDirection; // 移动方向
    private Rigidbody rb; // Rigidbody 组件
    private AudioSource audioSource; // 音频源

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // 获取 Rigidbody 组件
        rb.useGravity = true; // 确保重力被应用
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 锁定旋转

        audioSource = GetComponent<AudioSource>(); // 获取 AudioSource 组件
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 如果没有 AudioSource 组件，则添加一个
        }

        if (audioMixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = audioMixerGroup; // 设置混音器组
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
        isDashing = true; // 标记为正在冲刺
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            float currentSpeed = Mathf.Lerp(moveSpeed, dashSpeed, elapsedTime / dashDuration);
            Vector3 targetPosition = rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        isDashing = false; // 冲刺结束，取消标记
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
        float reductionFactor = 0.5f; // 减速比例
        rb.velocity *= reductionFactor; // 减速

        if (rb.velocity.magnitude < moveSpeed)
        {
            isDashing = false; // 如果速度过小，则停止冲刺
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
        transform.position = resetPosition; // 重置位置
        rb.velocity = Vector3.zero; // 重置速度

        if (resetSound != null)
        {
            audioSource.PlayOneShot(resetSound); // 播放重置音效
        }

        // 重置初始坐标
        moveDirection = Vector3.zero;
        dashTimer = 0f;
        isDashing = false;
    }
}
