using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload_B : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    public float resetYThreshold = -10f; // 设置一个阈值，当对象的y值低于这个值时重置位置

    // Start is called before the first frame update
    void Start()
    {
        // 记录初始位置、旋转和缩放
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < resetYThreshold)
        {
            // 重置位置、旋转和缩放
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            transform.localScale = initialScale;
        }
    }
}
