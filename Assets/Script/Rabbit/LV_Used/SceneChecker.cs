using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneChecker : MonoBehaviour
{
    public GameObject targetObject; // Ҫ����Ŀ������
    public int targetSceneIndex; // Ŀ�곡��������
    public Button checkButton; // ��ť�������

    void Start()
    {
        // ȷ����ť����¼��Ѱ�
        if (checkButton != null)
        {
            checkButton.onClick.AddListener(ActivateAndCheck);
        }
    }

    public void ActivateAndCheck()
    {
        // ���ǰ�ű����ڵ� GameObject
        gameObject.SetActive(true);
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
