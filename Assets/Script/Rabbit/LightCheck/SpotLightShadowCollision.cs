using System.Collections;
using UnityEngine;

public class SpotLightShadowCollision : MonoBehaviour
{
    public Light spotLight; // �۹�����
    public GameObject targetObject; // Ŀ������
    public float detectionResolution = 10; // ���߷ֲ����ܶ�
    public GameObject customObject; // �Զ�������
    public GameObject objectToActivate; // ��Ҫ���������
    public GameObject objectToDeactivate; // ��Ҫ�رյ�����
    public CanvasGroup customCanvasGroup; // �Զ��廭��
    public float transitionDuration = 1f; // ����Ч���ĳ���ʱ��

    private bool isColliding = false; // ��ײ״̬

    void Update()
    {
        if (spotLight != null && targetObject != null)
        {
            bool collisionDetected = CheckCollision();

            if (collisionDetected && !isColliding)
            {
                Debug.Log("���������������۹��ͶӰ������ײ��");
                isColliding = true;

                if (customObject != null && !customObject.activeSelf)
                {
                    customObject.SetActive(true);
                }

                if (objectToActivate != null)
                {
                    StartCoroutine(FadeIn(objectToActivate));
                }

                if (objectToDeactivate != null)
                {
                    StartCoroutine(FadeOut(objectToDeactivate));
                }
            }
            else if (!collisionDetected && isColliding)
            {
                isColliding = false;

                if (customObject != null && customObject.activeSelf)
                {
                    customObject.SetActive(false);
                }
            }
        }
    }

    private bool CheckCollision()
    {
        float angleStep = spotLight.spotAngle / detectionResolution;
        float halfAngle = spotLight.spotAngle / 2;

        for (int i = 0; i <= detectionResolution; i++)
        {
            float angle = halfAngle - i * angleStep;
            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * spotLight.transform.forward;

            if (Physics.Raycast(spotLight.transform.position, direction, out RaycastHit hit, spotLight.range))
            {
                if (hit.collider.gameObject == targetObject || hit.collider.transform.IsChildOf(targetObject.transform))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator FadeIn(GameObject obj)
    {
        if (customCanvasGroup == null)
        {
            yield break;
        }

        customCanvasGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            customCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        customCanvasGroup.alpha = 1f;
        obj.SetActive(true);
        customCanvasGroup.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(GameObject obj)
    {
        if (customCanvasGroup == null)
        {
            yield break;
        }

        customCanvasGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            customCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        customCanvasGroup.alpha = 0f;
        obj.SetActive(false);
        customCanvasGroup.gameObject.SetActive(false);
    }
}

