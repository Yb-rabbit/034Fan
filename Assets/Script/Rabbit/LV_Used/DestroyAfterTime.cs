using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime = 10.0f; // ��������ʱ�䣬Ĭ��Ϊ10��

    void Start()
    {
        // ����Destroy��������ָ��ʱ������ٵ�ǰ����
        Destroy(gameObject, destroyTime);
    }
}