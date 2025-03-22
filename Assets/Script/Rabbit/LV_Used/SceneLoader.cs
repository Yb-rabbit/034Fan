using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public int targetSceneIndex; // 目标场景索引
    public AudioClip audioClip; // 音频剪辑
    private AudioSource audioSource; // 音频源

    private void Start()
    {
        // 获取或添加 AudioSource 组件
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 触发器加载目标场景
        if (other.CompareTag("Ball"))
        {
            // 播放音频
            audioSource.clip = audioClip;
            audioSource.Play();

            // 启动协程等待音频播放完毕后加载场景
            StartCoroutine(LoadSceneAfterAudio());
        }
    }

    private IEnumerator LoadSceneAfterAudio()
    {
        // 等待音频播放完毕
        yield return new WaitForSeconds(audioSource.clip.length);

        // 加载目标场景
        SceneManager.LoadScene(targetSceneIndex);
    }
}
