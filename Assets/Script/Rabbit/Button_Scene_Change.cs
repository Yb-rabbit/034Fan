using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Button_Scene_Change : MonoBehaviour
{
    public int targetSceneIndex; // Ŀ�곡�������к�
    public float rotationSpeed = 10f; // ��ת�ٶ�
    public float scaleChange = 0.2f; // ��꿿��ʱ�����ű仯
    public float fadeTime = 2f; // ��Ļ��ڵ�ʱ��
    public Image blackScreen; // ȫ����ɫͼ��
    public Vector3 rotationAxis = Vector3.up; // �Զ�����ת��

    private float targetScale = 1f; // Ŀ������ֵ
    private float currentScale = 1f; // ��ǰ����ֵ
    private bool isPressed = false; // �Ƿ񱻰���
    private Image buttonImage; // ��ť��Image���
    private RectTransform rectTransform; // ��ť��RectTransform���
    private float randomRotation; // �����ת�Ƕ�
    private bool isRotating = false; // �Ƿ�������ת

    void Start()
    {
        buttonImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        randomRotation = Random.Range(-10f, 10f); // ��ʼ�������ת�Ƕ�

        // ȷ����ɫͼ���ʼ״̬��͸���Ĳ�����
        if (blackScreen != null)
        {
            blackScreen.color = new Color(0, 0, 0, 0);
            blackScreen.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // �޲���ʱ��Z����ת
        if (!isRotating)
        {
            transform.Rotate(new Vector3(0, 0, randomRotation) * (rotationSpeed * Time.deltaTime));
        }

        // ƽ�����Ű�ť
        currentScale = Mathf.Lerp(currentScale, targetScale, 5f * Time.deltaTime);
        rectTransform.localScale = new Vector3(currentScale, currentScale, 1);

        // �����ť�����£�2�����Ļ��ڲ��л�����
        if (isPressed)
        {
            StartCoroutine(FadeAndLoadScene());
            isPressed = false; // ���ð���״̬
        }
    }

    // ��꿿��ʱ�Ŵ�ť
    public void OnPointerEnter()
    {
        targetScale = 1f + scaleChange;
        if (!isRotating)
        {
            StartCoroutine(RotateToAngle(30f));
        }
    }

    // ����뿪ʱ��С��ť
    public void OnPointerExit()
    {
        targetScale = 1f;
        if (!isRotating)
        {
            StartCoroutine(RotateToAngle(-30f));
        }
    }

    // ��ť������ʱ����
    public void OnPointerClick()
    {
        isPressed = true;
    }

    // ��Ļ��ڲ��л�������Э��
    private IEnumerator FadeAndLoadScene()
    {
        // ���ú�ɫͼ��
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
        }

        // �������
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            if (blackScreen != null)
            {
                blackScreen.color = new Color(0, 0, 0, Mathf.Clamp01(elapsedTime / fadeTime));
            }
            yield return null;
        }

        // �л���ָ������
        SceneManager.LoadScene(targetSceneIndex);

        // ���䵭������
        elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            if (blackScreen != null)
            {
                blackScreen.color = new Color(0, 0, 0, 1 - Mathf.Clamp01(elapsedTime / fadeTime));
            }
            yield return null;
        }

        // ���ú�ɫͼ��
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(false);
        }
    }

    // ��ת��ָ���Ƕȵ�Э��
    private IEnumerator RotateToAngle(float angle)
    {
        isRotating = true;
        float startAngle = rectTransform.eulerAngles.y;
        float endAngle = startAngle + angle;
        float elapsedTime = 0f;
        float duration = 0.5f; // ��ת����ʱ��

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime / duration);
            rectTransform.eulerAngles = new Vector3(rectTransform.eulerAngles.x, currentAngle, rectTransform.eulerAngles.z);
            yield return null;
        }

        // ȷ����ת����ȷ��Ŀ��Ƕ�
        rectTransform.eulerAngles = new Vector3(rectTransform.eulerAngles.x, endAngle, rectTransform.eulerAngles.z);

        // ��ת��ԭʼ�Ƕ�
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = Mathf.Lerp(endAngle, startAngle, elapsedTime / duration);
            rectTransform.eulerAngles = new Vector3(rectTransform.eulerAngles.x, currentAngle, rectTransform.eulerAngles.z);
            yield return null;
        }

        // ȷ����ת��ԭʼ�Ƕ�
        rectTransform.eulerAngles = new Vector3(rectTransform.eulerAngles.x, startAngle, rectTransform.eulerAngles.z);
        isRotating = false;
    }
}
