using UnityEngine;
using UnityEngine.Audio;

public class PlayAudioSequence : MonoBehaviour
{
    public AudioClip audioClipA; // 音频A
    public AudioClip audioClipB; // 音频B
    public AudioMixerGroup mixerGroupA; // 混音器组A
    public AudioMixerGroup mixerGroupB; // 混音器组B
    private AudioSource audioSource;

    void Start()
    {
        // 获取或添加AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 检查是否已分配音频剪辑和混音器组
        if (audioClipA == null || audioClipB == null)
        {
            Debug.LogError("请在Inspector中分配音频剪辑A和B！");
            return;
        }

        if (mixerGroupA == null || mixerGroupB == null)
        {
            Debug.LogError("请在Inspector中分配混音器组A和B！");
            return;
        }

        // 播放音频A
        audioSource.clip = audioClipA;
        audioSource.outputAudioMixerGroup = mixerGroupA;
        audioSource.Play();

        // 在音频A播放完成后播放音频B
        Invoke("PlayAudioB", audioClipA.length + 0.8f);
    }

    void PlayAudioB()
    {
        // 播放音频B
        audioSource.clip = audioClipB;
        audioSource.outputAudioMixerGroup = mixerGroupB;
        audioSource.loop = true; // 设置音频B循环播放
        audioSource.Play();
    }
}
