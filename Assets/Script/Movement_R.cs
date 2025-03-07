using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_R : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f; // ��ת�ٶ�����

    [SerializeField]
    private float jumpForce = 5f; // ��Ծ��

    [SerializeField]
    private float smoothTime = 0.1f; // ƽ��ʱ������

    private float targetRotationX;
    private float currentRotationX;
    private float rotationVelocity;
    private bool isJumping; // �Ƿ�������Ծ
    private bool isPreparingJump; // �Ƿ�����׼����Ծ

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentRotationX = 0f;
        isJumping = false;
        isPreparingJump = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update ��ÿһ֡����һ��
    void Update()
    {
        // ��ⰴ��R��
        if (Input.GetKeyDown(KeyCode.R) && !isJumping && !isPreparingJump)
        {
            isPreparingJump = true;
            targetRotationX = 45f; // Ŀ����ת�Ƕ�
        }

        // ƽ����ת��Ŀ��Ƕ�
        if (isPreparingJump)
        {
            currentRotationX = Mathf.SmoothDamp(currentRotationX, targetRotationX, ref rotationVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(currentRotationX, 0, 0);

            // ����Ƿ��Ѿ���ת��Ŀ��Ƕ�
            if (Mathf.Abs(currentRotationX - targetRotationX) < 0.1f)
            {
                isPreparingJump = false;
                isJumping = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        // ��ת������
        if (isJumping)
        {
            targetRotationX += rotationSpeed * Time.deltaTime;
            currentRotationX = Mathf.SmoothDamp(currentRotationX, targetRotationX, ref rotationVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(currentRotationX, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ����Ƿ��ŵ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            targetRotationX = 0;
            currentRotationX = 0;
            rotationVelocity = 0;
            transform.rotation = Quaternion.identity;
        }
    }

    // ������ת����
    public void ResetRotation()
    {
        targetRotationX = 0;
        currentRotationX = 0;
        rotationVelocity = 0;
        isJumping = false;
        isPreparingJump = false;
        transform.rotation = Quaternion.identity;
    }

    // �ص�����ʱȡ�����и�������
    public void ResetToCheckpoint()
    {
        ResetRotation();
        rb.velocity = Vector3.zero; // �����ٶ�
        rb.angularVelocity = Vector3.zero; // ���ý��ٶ�
        transform.position = Vector3.zero; // ����λ��
    }

    // ����Ƿ����˶�
    public bool IsMoving()
    {
        return isJumping || isPreparingJump;
    }
}

