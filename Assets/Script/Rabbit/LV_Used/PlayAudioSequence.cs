using UnityEngine;
using UnityEngine.Audio;

public class PlayAudioSequence : MonoBehaviour
{
    public AudioClip audioClipA; // ��ƵA
    public AudioClip audioClipB; // ��ƵB
    public AudioMixerGroup mixerGroupA; // ��������A
    public AudioMixerGroup mixerGroupB; // ��������B
    private AudioSource audioSource;

    void Start()
    {
        // ��ȡ�����AudioSource���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ����Ƿ��ѷ�����Ƶ�����ͻ�������
        if (audioClipA == null || audioClipB == null)
        {
            Debug.LogError("����Inspector�з�����Ƶ����A��B��");
            return;
        }

        if (mixerGroupA == null || mixerGroupB == null)
        {
            Debug.LogError("����Inspector�з����������A��B��");
            return;
        }

        // ������ƵA
        audioSource.clip = audioClipA;
        audioSource.outputAudioMixerGroup = mixerGroupA;
        audioSource.Play();

        // ����ƵA������ɺ󲥷���ƵB
        Invoke("PlayAudioB", audioClipA.length + 0.8f);
    }

    void PlayAudioB()
    {
        // ������ƵB
        audioSource.clip = audioClipB;
        audioSource.outputAudioMixerGroup = mixerGroupB;
        audioSource.loop = true; // ������ƵBѭ������
        audioSource.Play();
    }
}
