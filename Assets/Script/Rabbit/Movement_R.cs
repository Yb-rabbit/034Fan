using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_R : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 120f; // ��ת�ٶ�����

    [SerializeField]
    private float jumpForce = 15f; // ��Ծ��

    [SerializeField]
    private float smoothTime = 0.1f; // ƽ��ʱ������

    private float targetRotationX;
    private float currentRotationX;
    private bool isJumping; // �Ƿ�������Ծ
    private bool isPreparingJump; // �Ƿ�����׼����Ծ
    private float initialRotationX; // ��ʼ��ת�Ƕ�
    private Quaternion initialRotation; // ��ʼ��ת

    private Rigidbody rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentRotationX = 0f;
        isJumping = false;
        isPreparingJump = false;
        mainCamera = Camera.main;
    }

    // Update ��ÿһ֡����һ��
    void Update()
    {
        // ��ⰴ��R��
        if (Input.GetKeyDown(KeyCode.R) && !isJumping && !isPreparingJump)
        {
            PrepareJump();
        }

        // ƽ����ת��Ŀ��Ƕ�
        if (isPreparingJump)
        {
            currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, Time.deltaTime / smoothTime);
            transform.rotation = Quaternion.Euler(currentRotationX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

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
            if (targetRotationX - initialRotationX >= 360f)
            {
                targetRotationX = initialRotationX + 360f; // ������ת�Ƕ�ΪһȦ
                isJumping = false; // ֹͣ��ת
            }
            currentRotationX = Mathf.Lerp(currentRotationX, targetRotationX, Time.deltaTime / smoothTime);
            transform.rotation = initialRotation * Quaternion.Euler(currentRotationX, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ����Ƿ��ŵ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            ResetRotation(); // ������ת״̬
        }
    }

    // ������ת����
    public void ResetRotation()
    {
        targetRotationX = 0;
        currentRotationX = 0;
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

    // ��������Ϊ������ĳ���
    private void CorrectOrientation()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; // ����Y���Ӱ��
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    // ���������������ⲿ�����Դ�����������
    public void PrepareJump()
    {
        if (!isJumping && !isPreparingJump)
        {
            isPreparingJump = true;
            targetRotationX = 45f; // Ŀ����ת�Ƕ�
            initialRotationX = currentRotationX; // ��¼��ʼ��ת�Ƕ�
            initialRotation = transform.rotation; // ��¼��ʼ��ת
            CorrectOrientation(); // ��������
        }
    }
}
