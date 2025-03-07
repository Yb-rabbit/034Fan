using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload_B : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    public float resetYThreshold = -10f; // ����һ����ֵ���������yֵ�������ֵʱ����λ��

    // Start is called before the first frame update
    void Start()
    {
        // ��¼��ʼλ�á���ת������
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < resetYThreshold)
        {
            // ����λ�á���ת������
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            transform.localScale = initialScale;
        }
    }
}
