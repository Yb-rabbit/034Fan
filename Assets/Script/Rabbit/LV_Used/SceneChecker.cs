using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneChecker : MonoBehaviour
{
    public GameObject targetObject; // Ҫ����Ŀ������
    public int targetSceneIndex; // Ŀ�곡��������
    public Button checkButton; // ��ť�������
    public Camera mainCamera; // �����������
    public float targetFOV = 90f; // Ŀ����Ұֵ
    public float fovChangeDuration = 2f; // ��Ұ�仯�ĳ���ʱ��
    private float originalFOV; // ԭʼ��Ұֵ

    void Start()
    {
        // ȷ����ť����¼��Ѱ�
        if (checkButton != null)
        {
            checkButton.onClick.AddListener(ActivateAndCheck);
        }

        // ��¼�������ԭʼ��Ұֵ
        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
        }

        // ���ĳ�����������¼�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ActivateAndCheck()
    {
        // ���ǰ�ű����ڵ� GameObject
        gameObject.SetActive(true);
        StartCoroutine(ChangeFOVAndLoadScene());
    }

    IEnumerator ChangeFOVAndLoadScene()
    {
        // ƽ���ظı����������Ұ
        if (mainCamera != null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fovChangeDuration)
            {
                mainCamera.fieldOfView = Mathf.Lerp(originalFOV, targetFOV, elapsedTime / fovChangeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            mainCamera.fieldOfView = targetFOV;
        }

        // �ȴ�
        yield return new WaitForSeconds(0.2f);

        // ���Ŀ�������Ƿ���ڲ�����Ŀ�곡��
        CheckAndLoadScene();
    }

    void CheckAndLoadScene()
    {
        // ���Ŀ�������Ƿ����
        if (targetObject != null && targetObject.activeInHierarchy)
        {
            Debug.Log("Ŀ��������ڣ�������ת��Ŀ�곡��");
            // ���Ŀ��������ڣ�ֱ�Ӽ���Ŀ�곡��
            SceneManager.LoadScene(targetSceneIndex);
        }
        else
        {
            Debug.Log("Ŀ�����岻���ڣ��޷���ת");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �������������Ұ
        if (mainCamera != null)
        {
            mainCamera.fieldOfView = originalFOV;
        }
    }

    void OnDestroy()
    {
        // ȡ�����ĳ�����������¼�
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

public class SceneFader : MonoBehaviour
{
    public Image fadeImage; // ���ڵ���Ч����Image���
    public float fadeDuration = 1.0f; // ����Ч���ĳ���ʱ��

    private void Start()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            StartCoroutine(FadeIn());
        }
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut(int sceneIndex)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        fadeImage.gameObject.SetActive(true);
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }
}
