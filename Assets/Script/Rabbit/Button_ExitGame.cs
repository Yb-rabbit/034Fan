using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Button_ExitGame : MonoBehaviour
{
    public float rotationSpeed = 10f; // 旋转速度
    public float scaleChange = 0.2f; // 鼠标靠近时的缩放变化
    public float fadeTime = 2f; // 屏幕变黑的时间
    public Image blackScreen; // 全屏黑色图像

    private float targetScale = 1f; // 目标缩放值
    private float currentScale = 1f; // 当前缩放值
    private bool isPressed = false; // 是否被按下
    private Image buttonImage; // 按钮的Image组件
    private RectTransform rectTransform; // 按钮的RectTransform组件
    private float randomRotation; // 随机旋转角度
    private bool isRotating = false; // 是否正在旋转

    void Start()
    {
        buttonImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        randomRotation = Random.Range(-10f, 10f); // 初始化随机旋转角度

        // 确保黑色图像初始状态是透明的并禁用
        if (blackScreen != null)
        {
            blackScreen.color = new Color(0, 0, 0, 0);
            blackScreen.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // 无操作时沿Z轴旋转
        if (!isRotating)
        {
            transform.Rotate(new Vector3(0, 0, randomRotation) * (rotationSpeed * Time.deltaTime));
        }

        // 平滑缩放按钮
        currentScale = Mathf.Lerp(currentScale, targetScale, 5f * Time.deltaTime);
        rectTransform.localScale = new Vector3(currentScale, currentScale, 1);

        // 如果按钮被按下，2秒后屏幕变黑并退出游戏
        if (isPressed)
        {
            StartCoroutine(FadeAndQuit());
            isPressed = false; // 重置按下状态
        }
    }

    // 鼠标靠近时放大按钮
    public void OnPointerEnter()
    {
        targetScale = 1f + scaleChange;
        if (!isRotating)
        {
            StartCoroutine(RotateToAngle(30f));
        }
    }

    // 鼠标离开时缩小按钮
    public void OnPointerExit()
    {
        targetScale = 1f;
        if (!isRotating)
        {
            StartCoroutine(RotateToAngle(-30f));
        }
    }

    // 按钮被按下时触发
    public void OnPointerClick()
    {
        isPressed = true;
    }

    // 屏幕变黑并退出游戏的协程
    private IEnumerator FadeAndQuit()
    {
        // 启用黑色图像
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);
        }

        // 渐变黑屏
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

        // 退出游戏或停止播放模式
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 旋转到指定角度的协程
    private IEnumerator RotateToAngle(float angle)
    {
        isRotating = true;
        float startAngle = rectTransform.eulerAngles.x;
        float endAngle = startAngle + angle;
        float elapsedTime = 0f;
        float duration = 0.5f; // 旋转持续时间

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime / duration);
            rectTransform.eulerAngles = new Vector3(currentAngle, rectTransform.eulerAngles.y, rectTransform.eulerAngles.z);
            yield return null;
        }

        // 确保旋转到精确的目标角度
        rectTransform.eulerAngles = new Vector3(endAngle, rectTransform.eulerAngles.y, rectTransform.eulerAngles.z);

        // 旋转回原始角度
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentAngle = Mathf.Lerp(endAngle, startAngle, elapsedTime / duration);
            rectTransform.eulerAngles = new Vector3(currentAngle, rectTransform.eulerAngles.y, rectTransform.eulerAngles.z);
            yield return null;
        }

        // 确保旋转回原始角度
        rectTransform.eulerAngles = new Vector3(startAngle, rectTransform.eulerAngles.y, rectTransform.eulerAngles.z);
        isRotating = false;
    }
}

