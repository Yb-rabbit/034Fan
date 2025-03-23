using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioClip collisionSound; // ײ��ʱ���ŵ���Ƶ����
    public string targetTag = "Target"; // Ŀ������ı�ǩ
    private AudioSource audioSource; // ���ڲ�����Ƶ��AudioSource���

    void Start()
    {
        // ȷ����ǰ��������AudioSource���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // ���û�У��Զ����
        }

        // ����AudioSource����Ƶ����
        audioSource.clip = collisionSound;
    }

    void OnCollisionEnter(Collision collision)
    {
        // �����巢����ײʱ�������ײ����ı�ǩ
        if (collision.gameObject.CompareTag(targetTag))
        {
            // ������Ƶ
            if (collisionSound != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("δ������Ƶ������");
            }
        }
    }
}
