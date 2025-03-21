using System.Collections;
using UnityEngine;

public class SpotLightShadowCollision : MonoBehaviour
{
    public Light spotLight; // 聚光灯组件
    public GameObject targetObject; // 目标物体
    public float detectionResolution = 10; // 射线分布的密度
    public GameObject customObject; // 自定义物体
    public GameObject objectToActivate; // 需要激活的物体
    public GameObject objectToDeactivate; // 需要关闭的物体
    public CanvasGroup customCanvasGroup; // 自定义画布
    public float transitionDuration = 1f; // 过渡效果的持续时间

    private bool isColliding = false; // 碰撞状态

    void Update()
    {
        if (spotLight != null && targetObject != null)
        {
            bool collisionDetected = CheckCollision();

            if (collisionDetected && !isColliding)
            {
                Debug.Log("物体或其子物体与聚光灯投影发生碰撞！");
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

