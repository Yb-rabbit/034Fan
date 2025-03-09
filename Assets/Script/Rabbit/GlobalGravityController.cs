using UnityEngine;

public class GlobalGravityController : MonoBehaviour
{
    [SerializeField]
    private Vector3 globalGravity = new Vector3(0, -9.81f, 0); // 默认重力

    void Start()
    {
        // 设置全局重力
        Physics.gravity = globalGravity;
    }

    void Update()
    {
        // 如果需要在运行时动态调整重力，可以在这里进行
        // Physics.gravity = globalGravity;
    }

    public void SetGlobalGravity(Vector3 newGravity)
    {
        globalGravity = newGravity;
        Physics.gravity = globalGravity;
    }
}
