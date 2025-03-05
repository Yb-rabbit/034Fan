using UnityEngine;

public class ForJump_A : MonoBehaviour
{
    public AudioClip[] jumpSounds; // ���һ����Ч�ļ����������
    private AudioSource audioSource;
    private Movement_B movementB;

    void Start()
    {
        // ��ȡ����� AudioSource ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ��ȡ Movement_B ���
        movementB = GetComponent<Movement_B>();
        if (movementB != null)
        {
            // ������Ծ�¼�
            movementB.OnJump += PlayJumpSound;
        }
    }

    void OnDestroy()
    {
        if (movementB != null)
        {
            // ȡ��������Ծ�¼�
            movementB.OnJump -= PlayJumpSound;
        }
    }

    private void PlayJumpSound()
    {
        if (jumpSounds != null && jumpSounds.Length > 0)
        {
            // ���ѡ��һ����Ч
            int index = UnityEngine.Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[index]);
        }
    }
}
