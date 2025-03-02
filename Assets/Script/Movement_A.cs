using UnityEngine;

public class Movement_A : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f; // ����
    [SerializeField]
    private float jumpForce = 10f; // ��Ծ��
    [SerializeField]
    private float jumpCooldown = 0.1f; // ��Ծ��Ľ����ƶ�ʱ��

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����
    private bool isJumping = false; // �Ƿ�����Ծ��
    private float jumpTimer = 0f; // ��Ծ��ʱ��

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // ȷ��������Ч
    }

    void Update()
    {
        // ��Ծ�߼�
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // ��ȡ�ƶ�����
        Vector3 movement = GetInputMovement();

        // ���������Ծ�л�����Ծ��Ľ����ƶ�ʱ���ڣ��淶���ƶ����򣨱���Խ����ƶ��ٶȹ��죩
        if (!isJumping || jumpTimer <= 0f)
        {
            if (movement != Vector3.zero)
            {
                movement.Normalize();
            }

            // �ƶ�����
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }

        // ������Ծ��ʱ��
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.fixedDeltaTime;
        }

        // ����Ƿ��ڵ�����
        CheckGrounded();
    }

    private Vector3 GetInputMovement()
    {
        Vector3 movement = Vector3.zero;

        if (jumpTimer <= 0f) // ֻ������Ծ��ʱ�����������Ӧ�ƶ�����
        {
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
        }

        return movement;
    }

    void Jump()
    {
        // ������ϵ�����ʵ����Ծ
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // ��Ծ�����ڵ�����
        isJumping = true; // ����Ϊ��Ծ��
        jumpTimer = jumpCooldown; // ������Ծ��ʱ��
    }

    void CheckGrounded()
    {
        // ʹ�����߼���Ƿ��ڵ�����
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
        if (isGrounded)
        {
            isJumping = false; // ��غ�����Ծ��
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // �������ı�ǩ��"Ground"
        {
            isGrounded = true; // ��������ʱ����Ϊ�ڵ�����
            isJumping = false; // ��غ�����Ծ��
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // �������ı�ǩ��"Ground"
        {
            isGrounded = false; // �뿪����ʱ����Ϊ���ڵ�����
        }
    }
}
