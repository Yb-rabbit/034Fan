using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 普通移动速度
    public float dashSpeed = 15f; // 冲刺最大速度
    public float dashDuration = 0.5f; // 冲刺持续时间
    public float dashCooldown = 1f; // 冲刺冷却时间
    private float dashTimer = 0f; // 冲刺计时器
    private bool isDashing = false; // 是否正在冲刺
    private Vector3 moveDirection; // 移动方向
    private Rigidbody rb; // Rigidbody 组件
    private float currentSpeed = 0f; // 当前速度

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // 获取 Rigidbody 组件
        rb.useGravity = true; // 确保重力被应用
        rb.constraints = RigidbodyConstraints.FreezeRotation; // 锁定旋转
    }

    private void FixedUpdate()
    {
        // 普通移动
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveY;
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
            // 更新小方块的面向
            transform.LookAt(transform.position + moveDirection, Vector3.up);
        }

        // 冲刺逻辑
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
            currentSpeed = Mathf.Lerp(moveSpeed, dashSpeed, elapsedTime / dashDuration);
            rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        // 冲刺结束，回到普通速度
        elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            currentSpeed = Mathf.Lerp(dashSpeed, moveSpeed, elapsedTime / dashDuration);
            rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        isDashing = false; // 冲刺结束，取消标记
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 当发生碰撞时，减速
        if (!isDashing)
        {
            rb.velocity *= 0.5f; // 减小速度
        }
    }
}
