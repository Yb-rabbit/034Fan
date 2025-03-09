using UnityEngine;

public class ForJump_A : MonoBehaviour
{
    public AudioClip[] jumpSounds; // 添加一个音效文件数组的引用
    private AudioSource audioSource;
    private Movement_B movementB;

    void Start()
    {
        // 获取或添加 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 获取 Movement_B 组件
        movementB = GetComponent<Movement_B>();
        if (movementB != null)
        {
            // 订阅跳跃事件
            movementB.OnJump += PlayJumpSound;
        }
    }

    void OnDestroy()
    {
        if (movementB != null)
        {
            // 取消订阅跳跃事件
            movementB.OnJump -= PlayJumpSound;
        }
    }

    private void PlayJumpSound()
    {
        if (jumpSounds != null && jumpSounds.Length > 0)
        {
            // 随机选择一个音效
            int index = UnityEngine.Random.Range(0, jumpSounds.Length);
            audioSource.PlayOneShot(jumpSounds[index]);
        }
    }
}
