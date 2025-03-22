using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public int targetSceneIndex; // Ŀ�곡������
    public AudioClip audioClip; // ��Ƶ����
    private AudioSource audioSource; // ��ƵԴ

    private void Start()
    {
        // ��ȡ����� AudioSource ���
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ����������Ŀ�곡��
        if (other.CompareTag("Ball"))
        {
            // ������Ƶ
            audioSource.clip = audioClip;
            audioSource.Play();

            // ����Э�̵ȴ���Ƶ������Ϻ���س���
            StartCoroutine(LoadSceneAfterAudio());
        }
    }

    private IEnumerator LoadSceneAfterAudio()
    {
        // �ȴ���Ƶ�������
        yield return new WaitForSeconds(audioSource.clip.length);

        // ����Ŀ�곡��
        SceneManager.LoadScene(targetSceneIndex);
    }
}
