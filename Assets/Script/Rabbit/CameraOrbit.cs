using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // ҪΧ�Ƶ�Ŀ������
    public float orbitSpeed = 90.0f; // ��ת�ٶ�
    public float distance = 5.0f; // �������Ŀ������֮��ľ���
    public float minDistance = 2.0f; // �������Ŀ������֮�����С����
    public float maxDistance = 10.0f; // �������Ŀ������֮���������
    public float height = 2.0f; // ������Ĺ̶��߶�
    private float currentAngle = 0.0f; // ��ǰ��ת�Ƕ�

    void Update()
    {
        // �������Ҽ�����
        if (Input.GetMouseButton(1))
        {
            // �Զ�ƽ����ת
            currentAngle += orbitSpeed * Time.deltaTime;
        }

        // �������������������
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * 2.0f; // ����������ٶ�

        // �������������С��������
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // �����µ�λ��
        Vector3 targetPosition = target.position;
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * distance; // ʹ�ö�̬����
        offset.y = height; // ���ù̶��߶�
        transform.position = targetPosition + offset;

        // �������ʼ�տ���Ŀ��
        transform.LookAt(target);
    }
}
