using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float fallSpeed = 1f; // �����ٶȣ���λ/��

    void Update()
    {
        // ʹ��������
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ֹͣ����
        fallSpeed = 0;
    }
}
