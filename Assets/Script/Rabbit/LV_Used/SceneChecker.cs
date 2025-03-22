using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneChecker : MonoBehaviour
{
    public GameObject targetObject; // 要检查的目标物体
    public int targetSceneIndex; // 目标场景的索引
    public Button checkButton; // 按钮组件引用

    void Start()
    {
        // 确保按钮点击事件已绑定
        if (checkButton != null)
        {
            checkButton.onClick.AddListener(ActivateAndCheck);
        }
    }

    public void ActivateAndCheck()
    {
        // 激活当前脚本所在的 GameObject
        gameObject.SetActive(true);
        CheckAndLoadScene();
    }

    void CheckAndLoadScene()
    {
        // 检查目标物体是否存在
        if (targetObject != null && targetObject.activeInHierarchy)
        {
            Debug.Log("目标物体存在，正在跳转到目标场景");
            // 如果目标物体存在，直接加载目标场景
            SceneManager.LoadScene(targetSceneIndex);
        }
        else
        {
            Debug.Log("目标物体不存在，无法跳转");
        }
    }
}

public class SceneFader : MonoBehaviour
{
    public Image fadeImage; // 用于淡出效果的Image组件
    public float fadeDuration = 1.0f; // 淡出效果的持续时间

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
