using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ParticleEffectAudioSwitcher : MonoBehaviour
{
    public ParticleSystem particleEffect; // 粒子系统
    public AudioSource audioSource; // 音频源
    public AudioClip audioClipA; // 音频A
    public AudioClip audioClipB; // 音频B
    public float switchDuration = 5f; // 自定义秒数
    public float levelSwitchDuration = 10f; // 切换关卡的自定义秒数
    public int nextSceneIndex; // 要切换到的场景索引
    public GameObject targetObject; // 需要逐渐缩小的对象
    public float shrinkDuration = 2f; // 缩小的持续时间

    private float effectStartTime; // 粒子效果开始时间
    private bool isEffectActive = false; // 粒子效果是否激活
    private bool audioSwitched = false; // 音频是否已切换
    private bool isShrinking = false; // 是否正在缩小

    void Start()
    {
        if (audioSource != null && audioClipA != null)
        {
            audioSource.clip = audioClipA; // 初始化音频源为音频A
            audioSource.Play();
        }
    }

    void Update()
    {
        if (particleEffect != null && particleEffect.isPlaying)
        {
            if (!isEffectActive)
            {
                // 粒子效果刚刚启用
                effectStartTime = Time.time;
                isEffectActive = true;
                audioSwitched = false; // 重置音频切换状态
                isShrinking = false; // 重置缩小状态
            }
            else
            {
                if (!audioSwitched && Time.time - effectStartTime >= switchDuration)
                {
                    // 粒子效果维持自定义秒数后切换音频
                    SwitchAudio();
                    audioSwitched = true; // 标记音频已切换
                }

                if (!isShrinking && Time.time - effectStartTime >= levelSwitchDuration)
                {
                    // 粒子效果维持自定义秒数后开始缩小并切换关卡
                    StartCoroutine(ShrinkAndSwitchLevel());
                    isShrinking = true; // 标记为正在缩小
                }
            }
        }
        else
        {
            isEffectActive = false; // 重置状态
        }
    }

    void SwitchAudio()
    {
        if (audioSource != null && audioClipB != null)
        {
            audioSource.clip = audioClipB; // 切换为音频B
            audioSource.Play();
        }
    }

    IEnumerator ShrinkAndSwitchLevel()
    {
        if (targetObject != null)
        {
            Vector3 originalScale = targetObject.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < shrinkDuration)
            {
                elapsedTime += Time.deltaTime;
                float scale = Mathf.Lerp(1f, 0f, elapsedTime / shrinkDuration);
                targetObject.transform.localScale = originalScale * scale;
                yield return null;
            }

            // 确保最终缩小为0
            targetObject.transform.localScale = Vector3.zero;
        }

        // 切换到指定索引的场景
        SceneManager.LoadScene(nextSceneIndex);
    }
}

