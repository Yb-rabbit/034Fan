using UnityEngine;

public class LockPositionOnGroundCollision : MonoBehaviour
{
    private float lockedYPosition; // 锁定的 Y 轴位置
    private bool isLocked = false; // 是否锁定位置

    void OnCollisionEnter(Collision collision) // 碰撞进入事件
    {
        if (collision.gameObject.CompareTag("Ground") && !isLocked) // 检测碰撞对象是否带有 "Ground" 标签并且未锁定
        {
            // 记录当前的 Y 轴位置
            lockedYPosition = transform.position.y;

            // 锁定 Y 轴位置
            isLocked = true;
        }
    }

    void Update()
    {
        if (isLocked)
        {
            // 锁定 Y 轴位置
            transform.position = new Vector3(transform.position.x, lockedYPosition, transform.position.z);
        }
    }
}
