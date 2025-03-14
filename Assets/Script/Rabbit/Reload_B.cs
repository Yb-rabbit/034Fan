using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload_B : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    public float resetYThreshold = -10f; // 设置一个阈值，当对象的y值低于这个值时重置位置
    public float resetDuration = 1f; // 重置过程的持续时间

    private bool isResetting = false; // 标记是否正在重置

    void Start()
    {
        // 记录初始位置、旋转和缩放
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (!isResetting && transform.position.y < resetYThreshold)
        {
            StartCoroutine(ResetPosition());
        }
    }

    private IEnumerator ResetPosition()
    {
        isResetting = true;

        // 优化读取位置和旋转
        transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
        Vector3 startScale = transform.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;

            // 平滑插值位置、旋转和缩放
            transform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, initialPosition, t),
                Quaternion.Slerp(startRotation, initialRotation, t)
            );
            transform.localScale = Vector3.Lerp(startScale, initialScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终位置、旋转和缩放完全重置
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        transform.localScale = initialScale;

        isResetting = false;
    }
}
