using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // 目标对象的Transform组件
    public Vector3 offset = new Vector3(0, 5, -10); // 摄像机与目标对象的偏移量，初始值为(0, 5, -10)
    public float smoothSpeed = 0.125f; // 平滑跟随的速度
    public float rotationSpeed = 5.0f; // 旋转速度
    public float minDistance = 2.0f; // 最小距离
    public float maxDistance = 20.0f; // 最大距离
    public float zoomSpeed = 2.0f; // 缩放速度

    private Vector3 velocity = Vector3.zero; // 用于插值计算的速度
    private float currentYAngle = 0f; // 当前Y轴旋转角度
    private float targetDistance; // 目标距离

    void Start()
    {
        if (target != null)
        {
            // 初始化摄像机的旋转角度，使其与目标对象的方向一致
            Vector3 lookPos = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;

            // 初始化当前Y轴旋转角度
            currentYAngle = transform.eulerAngles.y;

            // 初始化目标距离
            targetDistance = offset.magnitude;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("FollowTarget: No target assigned.");
            return;
        }

        HandleZoom();
        HandleRotation();
        HandlePosition();
    }

    private void HandleZoom()
    {
        // 获取鼠标滚轮输入
        float scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        targetDistance = Mathf.Clamp(targetDistance - scroll, minDistance, maxDistance);

        // 平滑调整摄像机的距离
        float currentDistance = offset.magnitude;
        float newDistance = Mathf.Lerp(currentDistance, targetDistance, smoothSpeed);
        offset = offset.normalized * newDistance;
    }

    private void HandleRotation()
    {
        // 获取用户输入
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;

        // 旋转摄像机
        currentYAngle += horizontal;

        // 应用旋转
        Quaternion rotation = Quaternion.Euler(0, currentYAngle, 0);
        transform.rotation = rotation;
    }

    private void HandlePosition()
    {
        // 平滑跟随目标对象
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        // 使摄像机始终朝向目标对象
        transform.LookAt(target.position + Vector3.up * offset.y);
    }
}
