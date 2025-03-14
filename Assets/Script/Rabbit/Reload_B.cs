using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload_B : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    public float resetYThreshold = -10f; // ����һ����ֵ���������yֵ�������ֵʱ����λ��
    public float resetDuration = 1f; // ���ù��̵ĳ���ʱ��

    private bool isResetting = false; // ����Ƿ���������

    void Start()
    {
        // ��¼��ʼλ�á���ת������
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

        // �Ż���ȡλ�ú���ת
        transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
        Vector3 startScale = transform.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < resetDuration)
        {
            float t = elapsedTime / resetDuration;

            // ƽ����ֵλ�á���ת������
            transform.SetPositionAndRotation(
                Vector3.Lerp(startPosition, initialPosition, t),
                Quaternion.Slerp(startRotation, initialRotation, t)
            );
            transform.localScale = Vector3.Lerp(startScale, initialScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ȷ������λ�á���ת��������ȫ����
        transform.SetPositionAndRotation(initialPosition, initialRotation);
        transform.localScale = initialScale;

        isResetting = false;
    }
}
