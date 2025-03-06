using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_R : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 100f; // ��ת�ٶ�����

    [SerializeField]
    private float smoothTime = 0.1f; // ƽ��ʱ������

    private float targetRotationY;
    private float currentRotationY;
    private float rotationVelocity;

    // Update ��ÿһ֡����һ��
    void Update()
    {
        // ��ȡ����������
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        // ���ݹ�����������Ŀ����ת�Ƕ�
        if (scrollInput != 0)
        {
            targetRotationY += scrollInput * rotationSpeed;
        }

        // ƽ����ֵ��ǰ��ת�Ƕȵ�Ŀ����ת�Ƕ�
        currentRotationY = Mathf.SmoothDamp(currentRotationY, targetRotationY, ref rotationVelocity, smoothTime);

        // Ӧ����ת
        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }

    // ������ת����
    public void ResetRotation()
    {
        targetRotationY = 0;
        currentRotationY = 0;
        rotationVelocity = 0;
        transform.rotation = Quaternion.identity;
    }
}
