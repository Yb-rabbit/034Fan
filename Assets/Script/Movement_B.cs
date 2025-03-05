using UnityEngine;

public class Movement_B : MonoBehaviour
{
    [SerializeField]
    private float groundSpeed = 3f; // �����ϵ�����
    [SerializeField]
    private float airSpeed = 1.5f; // ���е�����
    [SerializeField]
    private float jumpForce = 10f; // ��Ծ��

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // ȷ��������Ч

        // ��ʼ������
        CheckGroundStatus();
    }

    void FixedUpdate()
    {
        // ��ȡ�ƶ�����
        Vector3 movement = GetInputMovement();

        // �����Ƿ��ڵ����ϵ����ٶ�
        float currentSpeed = isGrounded ? groundSpeed : airSpeed;

        // �淶���ƶ����򣨱���Խ����ƶ��ٶȹ��죩
        if (movement != Vector3.zero)
        {
            movement.Normalize();
        }

        // �ƶ�����
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        // ��Ծ�߼�
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
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

    void Jump()
    {
        // ������ϵ�����ʵ����Ծ
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // ��Ծ�����ڵ�����
    }

    // �����⣨��Ҫһ��������ű�����Unity�༭����������ײ����
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // �������ı�ǩ��"Ground"
        {
            isGrounded = true; // ��������ʱ����Ϊ�ڵ�����
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // �������ı�ǩ��"Ground"
        {
            isGrounded = true; // ��������ʱ����Ϊ�ڵ�����
        }
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
