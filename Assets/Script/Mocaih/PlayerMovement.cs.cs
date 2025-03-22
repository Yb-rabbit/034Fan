using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // ��ͨ�ƶ��ٶ�
    public float dashSpeed = 15f; // �������ٶ�
    public float dashDuration = 0.5f; // ��̳���ʱ��
    public float dashCooldown = 1f; // �����ȴʱ��
    private float dashTimer = 0f; // ��̼�ʱ��
    private bool isDashing = false; // �Ƿ����ڳ��
    private Vector3 moveDirection; // �ƶ�����
    private Rigidbody rb; // Rigidbody ���
    private float currentSpeed = 0f; // ��ǰ�ٶ�

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // ��ȡ Rigidbody ���
        rb.useGravity = true; // ȷ��������Ӧ��
        rb.constraints = RigidbodyConstraints.FreezeRotation; // ������ת
    }

    private void FixedUpdate()
    {
        // ��ͨ�ƶ�
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        moveDirection = transform.right * moveX + transform.forward * moveY;
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
        }

        // ����߼�
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
        float initialSpeed = rb.velocity.magnitude;

        while (elapsedTime < dashDuration)
        {
            currentSpeed = Mathf.Lerp(moveSpeed, dashSpeed, elapsedTime / dashDuration);
            Vector3 targetPosition = rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        isDashing = false; // ��̽�����ȡ�����
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ��������ײʱ������
        if (isDashing)
        {
            float reductionFactor = 0.5f; // ���ٱ���
            Vector3 newVelocity = rb.velocity * reductionFactor;
            rb.velocity = newVelocity;
            // ����ٶȹ�С����ֹͣ���
            if (rb.velocity.magnitude < moveSpeed)
            {
                isDashing = false;
            }
        }
    }
}
