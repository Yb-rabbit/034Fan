using UnityEngine;
using System.Collections.Generic;

public class FollowTarget : MonoBehaviour
{
    public List<Transform> targets; // 目标对象的Transform组件列表
    public Vector3 offset = new Vector3(0, 5, -10); // 摄像机与目标对象的偏移量，初始值为(0, 5, -10)
    public float smoothSpeed = 0.125f; // 平滑跟随的速度
    public float rotationSpeed = 5.0f; // 旋转速度
    public float minDistance = 2.0f; // 最小距离
    public float maxDistance = 20.0f; // 最大距离
    public float zoomSpeed = 2.0f; // 缩放速度

    private Vector3 velocity = Vector3.zero; // 用于插值计算的速度
    private float currentYAngle = 0f; // 当前Y轴旋转角度
    private float targetDistance; // 目标距离
    private int currentTargetIndex = 0; // 当前目标对象的索引

    void Start()
    {
        if (targets != null && targets.Count > 0)
        {
            SetTarget(targets[currentTargetIndex]);
        }
    }

    void LateUpdate()
    {
        if (targets == null || targets.Count == 0)
        {
            Debug.LogWarning("FollowTarget: No targets assigned.");
            return;
        }

        if (Input.GetMouseButtonDown(1)) // 右键点击
        {
            SwitchTarget();
        }

        HandleZoom();
        HandleRotation();
        HandlePosition();
    }

    private void SetTarget(Transform newTarget)
    {
        if (newTarget != null)
        {
            // 初始化摄像机的旋转角度，使其与目标对象的方向一致
            Vector3 lookPos = newTarget.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;

            // 初始化当前Y轴旋转角度
            currentYAngle = transform.eulerAngles.y;

            // 初始化目标距离
            targetDistance = offset.magnitude;
        }
    }

    public void SwitchTarget()
    {
        currentTargetIndex = (currentTargetIndex + 1) % targets.Count;
        SetTarget(targets[currentTargetIndex]);
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
        Vector3 targetPosition = targets[currentTargetIndex].position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        // 使摄像机始终朝向目标对象
        transform.LookAt(targets[currentTargetIndex].position);
    }
}
