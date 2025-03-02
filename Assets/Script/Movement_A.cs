using UnityEngine;

public class Movement_A : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // 移速
    [SerializeField]
    private float jumpForce = 5f; // 跳跃高度

    private Rigidbody rb; // 刚体
    private bool isGrounded = false; // 是否在地面上

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //跳跃逻辑
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // 获取移动方向
        Vector3 movement = GetInputMovement();

        // 规范化移动方向（避免对角线移动速度过快）
        if (movement != Vector3.zero)
        {
            movement.Normalize();
        }

        // 移动物体
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private Vector3 GetInputMovement()
    {
        Vector3 movement = Vector3.zero;

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
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 假设地面的标签是"Ground"
        {
            isGrounded = true; // 碰到地面时设置为在地面上
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            isGrounded = false; // 离开地面时设置为不在地面上
        }
    }
}
