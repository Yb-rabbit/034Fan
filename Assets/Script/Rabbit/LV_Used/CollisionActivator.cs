using UnityEngine;

public class CollisionActivator : MonoBehaviour
{
    public GameObject objectC; // Ҫ���������C
    public GameObject objectB; // ����B������
    public AudioClip collisionSound; // ��ײʱ���ŵ���Ƶ
    private AudioSource audioSource; // ��ƵԴ

    private void Start()
    {
        // ��ȡ�����AudioSource���
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �����ײ�������Ƿ�������B
        if (collision.gameObject == objectB)
        {
            // ���������B����������C��������Ƶ
            if (objectC != null)
            {
                objectC.SetActive(true); // ��������C
                Debug.Log("����A������B��ײ������C�Ѽ���");
            }

            // ������ײ��Ƶ
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
                Debug.Log("������ײ��Ƶ");
            }
        }
    }
}
