using UnityEngine;

public class Movement_A : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f; // 移速
    [SerializeField]
    private float jumpForce = 10f; // 跳跃力
    [SerializeField]
    private float jumpCooldown = 0.1f; // 跳跃后的禁用移动时间

    private Rigidbody rb; // 刚体
    private bool isGrounded = false; // 是否在地面上
    private bool isJumping = false; // 是否在跳跃中
    private float jumpTimer = 0f; // 跳跃计时器

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // 确保重力生效
    }

    void Update()
    {
        // 跳跃逻辑
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // 获取移动方向
        Vector3 movement = GetInputMovement();

        // 如果不在跳跃中或不在跳跃后的禁用移动时间内，规范化移动方向（避免对角线移动速度过快）
        if (!isJumping || jumpTimer <= 0f)
        {
            if (movement != Vector3.zero)
            {
                movement.Normalize();
            }

            // 移动物体
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }

        // 更新跳跃计时器
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.fixedDeltaTime;
        }

        // 检测是否在地面上
        CheckGrounded();
    }

    private Vector3 GetInputMovement()
    {
        Vector3 movement = Vector3.zero;

        if (jumpTimer <= 0f) // 只有在跳跃计时器结束后才响应移动输入
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) //往前
            {
                movement += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) //往后
            {
                movement += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) //往左
            {
                movement += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) //往右
            {
                movement += Vector3.right;
            }
        }

        return movement;
    }

    void Jump()
    {
        // 添加向上的力来实现跳跃
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // 跳跃后不再在地面上
        isJumping = true; // 设置为跳跃中
        jumpTimer = jumpCooldown; // 重置跳跃计时器
    }

    void CheckGrounded()
    {
        // 使用射线检测是否在地面上
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
        if (isGrounded)
        {
            isJumping = false; // 落地后不再跳跃中
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = true; // 碰到地面时设置为在地面上
            isJumping = false; // 落地后不再跳跃中
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = false; // 离开地面时设置为不在地面上
        }
    }
}
