using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_RotationOnly : MonoBehaviour
{
    // ��ת�ٶ�
    public Vector3 rotationSpeed = new(10f, 20f, 30f);

    // Update is called once per frame
    void Update()
    {
        // ÿ֡��ת������
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
