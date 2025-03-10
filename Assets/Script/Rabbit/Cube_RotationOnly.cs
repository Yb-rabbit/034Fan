using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_RotationOnly : MonoBehaviour
{
    // 旋转速度
    public Vector3 rotationSpeed = new(10f, 20f, 30f);

    // Update is called once per frame
    void Update()
    {
        // 每帧旋转立方体
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
