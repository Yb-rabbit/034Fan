using UnityEngine;

public class Movement_B : MonoBehaviour
{
    [SerializeField]
    private float groundSpeed = 3f; // 地面上的移速
    [SerializeField]
    private float airSpeed = 1.5f; // 空中的移速
    [SerializeField]
    private float jumpForce = 10f; // 跳跃力
    [SerializeField]
    private float acceleration = 5f; // 加速度

    private Rigidbody rb; // 刚体
    private bool isGrounded = false; // 是否在地面上
    private Vector3 currentVelocity = Vector3.zero; // 当前速度

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // 确保重力生效

        // 初始地面检测
        CheckGroundStatus();
    }

    void Update()
    {
        // 跳跃逻辑
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // 获取移动方向
        Vector3 targetVelocity = GetInputMovement();

        // 根据是否在地面上调整速度
        float targetSpeed = isGrounded ? groundSpeed : airSpeed;

        // 规范化移动方向（避免对角线移动速度过快）
        if (targetVelocity != Vector3.zero)
        {
            targetVelocity.Normalize();
        }

        // 计算目标速度
        targetVelocity *= targetSpeed;

        // 使用 Lerp 实现平滑加速度
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // 移动物体
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    private Vector3 GetInputMovement()
    {
        Vector3 movement = Vector3.zero;

        // 响应移动输入
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

        return movement;
    }

    void Jump()
    {
        // 添加向上的力来实现跳跃
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // 跳跃后不再在地面上
    }

    // 地面检测（需要一个地面检测脚本或在Unity编辑器中设置碰撞器）
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = true; // 碰到地面时设置为在地面上
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = true; // 碰到地面时设置为在地面上
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = false; // 离开地面时设置为不在地面上
        }
    }

    // 初始地面检测
    private void CheckGroundStatus()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
    }
}
