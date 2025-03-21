using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // 要围绕的目标物体
    public float orbitSpeed = 90.0f; // 旋转速度
    public float distance = 5.0f; // 摄像机与目标物体之间的距离
    public float minDistance = 2.0f; // 摄像机与目标物体之间的最小距离
    public float maxDistance = 10.0f; // 摄像机与目标物体之间的最大距离
    public float height = 2.0f; // 摄像机的固定高度
    private float currentAngle = 0.0f; // 当前旋转角度

    void Update()
    {
        // 检测鼠标右键按下
        if (Input.GetMouseButton(1))
        {
            // 自动平滑旋转
            currentAngle += orbitSpeed * Time.deltaTime;
        }

        // 检测鼠标滚轮来调整距离
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * 2.0f; // 调整距离的速度

        // 限制摄像机的最小和最大距离
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // 计算新的位置
        Vector3 targetPosition = target.position;
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * distance; // 使用动态距离
        offset.y = height; // 设置固定高度
        transform.position = targetPosition + offset;

        // 让摄像机始终看向目标
        transform.LookAt(target);
    }
}
