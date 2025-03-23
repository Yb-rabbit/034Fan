using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime = 10.0f; // 设置销毁时间，默认为10秒

    void Start()
    {
        // 调用Destroy方法，在指定时间后销毁当前物体
        Destroy(gameObject, destroyTime);
    }
}