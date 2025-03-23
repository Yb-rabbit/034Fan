using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticleEffectAudioSwitcher : MonoBehaviour
{
    public ParticleSystem particleEffect; // 粒子系统
    public AudioSource audioSource; // 音频源
    public AudioClip audioClipA; // 音频A
    public AudioClip audioClipB; // 音频B
    public float switchDuration = 5f; // 自定义秒数
    public float levelSwitchDuration = 10f; // 切换关卡的自定义秒数
    public int nextSceneIndex; // 要切换到的场景索引

    private float effectStartTime; // 粒子效果开始时间
    private bool isEffectActive = false; // 粒子效果是否激活
    private bool audioSwitched = false; // 音频是否已切换

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
            }
            else
            {
                if (!audioSwitched && Time.time - effectStartTime >= switchDuration)
                {
                    // 粒子效果维持自定义秒数后切换音频
                    SwitchAudio();
                    audioSwitched = true; // 标记音频已切换
                }

                if (Time.time - effectStartTime >= levelSwitchDuration)
                {
                    // 粒子效果维持自定义秒数后切换关卡
                    SwitchLevel();
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

    void SwitchLevel()
    {
        SceneManager.LoadScene(nextSceneIndex); // 切换到指定索引的场景
    }
}
