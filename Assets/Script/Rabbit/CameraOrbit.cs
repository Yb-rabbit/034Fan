using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // 要围绕的目标物体
    public float orbitSpeed = 90.0f; // 旋转速度
    public float distance = 5.0f; // 摄像机与目标物体之间的距离
    public float minDistance = 2.0f; // 摄像机与目标物体之间的最小距离
    public float maxDistance = 10.0f; // 摄像机与目标物体之间的最大距离
    public float height = 2.0f; // 摄像机的固定高度
    public float smoothSpeed = 5.0f; // 平滑速度
    private float currentAngle = 0.0f; // 当前旋转角度
    private float targetDistance; // 目标距离

    void Start()
    {
        targetDistance = distance; // 初始化目标距离
    }

    void Update()
    {
        // 检测鼠标右键按下
        if (Input.GetMouseButton(1))
        {
            // 自动平滑旋转
            currentAngle += orbitSpeed * Time.deltaTime;
        }

        // 检测鼠标滚轮来调整目标距离
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetDistance -= scroll * 2.0f; // 调整目标距离的速度

        // 限制目标距离的最小和最大值
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        // 平滑过渡到目标距离
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * smoothSpeed);

        // 计算新的位置
        Vector3 targetPosition = target.position;
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), 0, Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * distance; // 使用动态距离
        offset.y = height; // 设置固定高度
        transform.position = targetPosition + offset;

        // 让摄像机始终看向目标
        transform.LookAt(target);
    }
}
