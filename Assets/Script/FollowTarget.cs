using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // Ŀ������Transform���
    public Vector3 offset = new Vector3(0, 5, -10); // �������Ŀ������ƫ��������ʼֵΪ(0, 5, -10)
    public float smoothSpeed = 0.125f; // ƽ��������ٶ�
    public float rotationSpeed = 5.0f; // ��ת�ٶ�
    public float minDistance = 2.0f; // ��С����
    public float maxDistance = 20.0f; // ������
    public float zoomSpeed = 2.0f; // �����ٶ�

    private Vector3 velocity = Vector3.zero; // ���ڲ�ֵ������ٶ�
    private float currentYAngle = 0f; // ��ǰY����ת�Ƕ�
    private float targetDistance; // Ŀ�����

    void Start()
    {
        if (target != null)
        {
            // ��ʼ�����������ת�Ƕȣ�ʹ����Ŀ�����ķ���һ��
            Vector3 lookPos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;

            // ��ʼ����ǰY����ת�Ƕ�
            currentYAngle = transform.eulerAngles.y;

            // ��ʼ��Ŀ�����
            targetDistance = offset.magnitude;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("FollowTarget: No target assigned.");
            return;
        }

        HandleZoom();
        HandleRotation();
        HandlePosition();
    }

    private void HandleZoom()
    {
        // ��ȡ����������
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        targetDistance = Mathf.Clamp(targetDistance - scroll, minDistance, maxDistance);

        // ƽ������������ľ���
        float currentDistance = offset.magnitude;
        float newDistance = Mathf.Lerp(currentDistance, targetDistance, smoothSpeed);
        offset = offset.normalized * newDistance;
    }

    private void HandleRotation()
    {
        // ��ȡ�û�����
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;

        // ��ת�����
        currentYAngle += horizontal;

        // Ӧ����ת
        Quaternion rotation = Quaternion.Euler(0, currentYAngle, 0);
        transform.rotation = rotation;
    }

    private void HandlePosition()
    {
        // ƽ������Ŀ�����
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        // ʹ�����ʼ�ճ���Ŀ�����
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}
