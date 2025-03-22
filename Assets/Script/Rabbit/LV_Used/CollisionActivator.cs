using UnityEngine;

public class CollisionActivator : MonoBehaviour
{
    public GameObject objectC; // 要激活的物体C
    public GameObject objectB; // 物体B的引用
    public AudioClip collisionSound; // 碰撞时播放的音频
    private AudioSource audioSource; // 音频源

    private void Start()
    {
        // 获取或添加AudioSource组件
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞的物体是否是物体B
        if (collision.gameObject == objectB)
        {
            // 如果是物体B，激活物体C并播放音频
            if (objectC != null)
            {
                objectC.SetActive(true); // 激活物体C
                Debug.Log("物体A与物体B碰撞，物体C已激活");
            }

            // 播放碰撞音频
            if (collisionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collisionSound);
                Debug.Log("播放碰撞音频");
            }
        }
    }
}
