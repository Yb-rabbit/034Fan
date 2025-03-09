using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float fallSpeed = 1f; // 下落速度，单位/秒

    void Update()
    {
        // 使方块下落
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 停止下落
        fallSpeed = 0;
    }
}
