using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload_B : MonoBehaviour
{
    private Vector3 initialPosition;
    public float resetYThreshold = -10f; // 设置一个阈值，当对象的y值低于这个值时重置位置

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position; // 记录初始位置
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < resetYThreshold)
        {
            transform.position = initialPosition; // 重置位置
        }
    }
}
