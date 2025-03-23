using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ParticleEffectAudioSwitcher : MonoBehaviour
{
    public ParticleSystem particleEffect; // ����ϵͳ
    public AudioSource audioSource; // ��ƵԴ
    public AudioClip audioClipA; // ��ƵA
    public AudioClip audioClipB; // ��ƵB
    public float switchDuration = 5f; // �Զ�������
    public float levelSwitchDuration = 10f; // �л��ؿ����Զ�������
    public int nextSceneIndex; // Ҫ�л����ĳ�������
    public GameObject targetObject; // ��Ҫ����С�Ķ���
    public float shrinkDuration = 2f; // ��С�ĳ���ʱ��

    private float effectStartTime; // ����Ч����ʼʱ��
    private bool isEffectActive = false; // ����Ч���Ƿ񼤻�
    private bool audioSwitched = false; // ��Ƶ�Ƿ����л�
    private bool isShrinking = false; // �Ƿ�������С

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
                isShrinking = false; // ������С״̬
            }
            else
            {
                if (!audioSwitched && Time.time - effectStartTime >= switchDuration)
                {
                    // ����Ч��ά���Զ����������л���Ƶ
                    SwitchAudio();
                    audioSwitched = true; // �����Ƶ���л�
                }

                if (!isShrinking && Time.time - effectStartTime >= levelSwitchDuration)
                {
                    // ����Ч��ά���Զ���������ʼ��С���л��ؿ�
                    StartCoroutine(ShrinkAndSwitchLevel());
                    isShrinking = true; // ���Ϊ������С
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

            // ȷ��������СΪ0
            targetObject.transform.localScale = Vector3.zero;
        }

        // �л���ָ�������ĳ���
        SceneManager.LoadScene(nextSceneIndex);
    }
}

