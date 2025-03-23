using UnityEngine;

public class LockPositionOnGroundCollision : MonoBehaviour
{
    private float lockedYPosition; // ������ Y ��λ��
    private bool isLocked = false; // �Ƿ�����λ��

    void OnCollisionEnter(Collision collision) // ��ײ�����¼�
    {
        if (collision.gameObject.CompareTag("Ground") && !isLocked) // �����ײ�����Ƿ���� "Ground" ��ǩ����δ����
        {
            // ��¼��ǰ�� Y ��λ��
            lockedYPosition = transform.position.y;

            // ���� Y ��λ��
            isLocked = true;
        }
    }

    void Update()
    {
        if (isLocked)
        {
            // ���� Y ��λ��
            transform.position = new Vector3(transform.position.x, lockedYPosition, transform.position.z);
        }
    }
}
