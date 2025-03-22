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
            // ����С���������
            transform.LookAt(transform.position + moveDirection, Vector3.up);
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

        while (elapsedTime < dashDuration)
        {
            currentSpeed = Mathf.Lerp(moveSpeed, dashSpeed, elapsedTime / dashDuration);
            rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        // ��̽������ص���ͨ�ٶ�
        elapsedTime = 0f;
        while (elapsedTime < dashDuration)
        {
            currentSpeed = Mathf.Lerp(dashSpeed, moveSpeed, elapsedTime / dashDuration);
            rb.MovePosition(rb.position + direction.normalized * currentSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        isDashing = false; // ��̽�����ȡ�����
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ��������ײʱ������
        if (!isDashing)
        {
            rb.velocity *= 0.5f; // ��С�ٶ�
        }
    }
}
