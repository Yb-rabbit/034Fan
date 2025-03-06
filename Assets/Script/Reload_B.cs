using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload_B : MonoBehaviour
{
    private Vector3 initialPosition;
    public float resetYThreshold = -10f; // ����һ����ֵ���������yֵ�������ֵʱ����λ��

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position; // ��¼��ʼλ��
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < resetYThreshold)
        {
            transform.position = initialPosition; // ����λ��
        }
    }
}
