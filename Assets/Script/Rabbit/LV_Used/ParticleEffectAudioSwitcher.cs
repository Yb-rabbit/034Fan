using UnityEngine;
using UnityEngine.SceneManagement;

public class ParticleEffectAudioSwitcher : MonoBehaviour
{
    public ParticleSystem particleEffect; // ����ϵͳ
    public AudioSource audioSource; // ��ƵԴ
    public AudioClip audioClipA; // ��ƵA
    public AudioClip audioClipB; // ��ƵB
    public float switchDuration = 5f; // �Զ�������
    public float levelSwitchDuration = 10f; // �л��ؿ����Զ�������
    public int nextSceneIndex; // Ҫ�л����ĳ�������

    private float effectStartTime; // ����Ч����ʼʱ��
    private bool isEffectActive = false; // ����Ч���Ƿ񼤻�
    private bool audioSwitched = false; // ��Ƶ�Ƿ����л�

    void Start()
    {
        if (audioSource != null && audioClipA != null)
        {
            audioSource.clip = audioClipA; // ��ʼ����ƵԴΪ��ƵA
            audioSource.Play();
        }
    }

    void Update()
    {
        if (particleEffect != null && particleEffect.isPlaying)
        {
            if (!isEffectActive)
            {
                // ����Ч���ո�����
                effectStartTime = Time.time;
                isEffectActive = true;
                audioSwitched = false; // ������Ƶ�л�״̬
            }
            else
            {
                if (!audioSwitched && Time.time - effectStartTime >= switchDuration)
                {
                    // ����Ч��ά���Զ����������л���Ƶ
                    SwitchAudio();
                    audioSwitched = true; // �����Ƶ���л�
                }

                if (Time.time - effectStartTime >= levelSwitchDuration)
                {
                    // ����Ч��ά���Զ����������л��ؿ�
                    SwitchLevel();
                }
            }
        }
        else
        {
            isEffectActive = false; // ����״̬
        }
    }

    void SwitchAudio()
    {
        if (audioSource != null && audioClipB != null)
        {
            audioSource.clip = audioClipB; // �л�Ϊ��ƵB
            audioSource.Play();
        }
    }

    void SwitchLevel()
    {
        SceneManager.LoadScene(nextSceneIndex); // �л���ָ�������ĳ���
    }
}
