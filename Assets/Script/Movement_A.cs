using UnityEngine;

public class Movement_A : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // ����
    [SerializeField]
    private float jumpForce = 5f; // ��Ծ�߶�

    private Rigidbody rb; // ����
    private bool isGrounded = false; // �Ƿ��ڵ�����

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //��Ծ�߼�
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // ��ȡ�ƶ�����
        Vector3 movement = GetInputMovement();

        // �淶���ƶ����򣨱���Խ����ƶ��ٶȹ��죩
        if (movement != Vector3.zero)
        {
            movement.Normalize();
        }

        // �ƶ�����
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private Vector3 GetInputMovement()
    {
        Vector3 movement = Vector3.zero;

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
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // �������ı�ǩ��"Ground"
        {
            isGrounded = true; // ��������ʱ����Ϊ�ڵ�����
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            isGrounded = false; // �뿪����ʱ����Ϊ���ڵ�����
        }
    }
}
