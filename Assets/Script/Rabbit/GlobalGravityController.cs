using UnityEngine;

public class GlobalGravityController : MonoBehaviour
{
    [SerializeField]
    private Vector3 globalGravity = new Vector3(0, -9.81f, 0); // Ĭ������

    void Start()
    {
        // ����ȫ������
        Physics.gravity = globalGravity;
    }

    void Update()
    {
        // �����Ҫ������ʱ��̬�����������������������
        // Physics.gravity = globalGravity;
    }

    public void SetGlobalGravity(Vector3 newGravity)
    {
        globalGravity = newGravity;
        Physics.gravity = globalGravity;
    }
}
