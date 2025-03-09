using UnityEngine;
using UnityEngine.Audio;

public class D_CubeController : MonoBehaviour
{
    private float scaleOfCube;
    private Rigidbody rb;
    private bool isFlipping;
    private Vector3 flipAxis;
    private Vector3 pivotPoint;
    private float currentAngle;
    public float RotateSpeed = 90f;
    public float fallThreshold = -10f; // 触发回到初始位置的 Y 值
    private Vector3 initialPosition; // 初始位置
    private AudioSource audioSource; // 音效组件
    public AudioClip fallSound; // 自定义音效
    public AudioMixerGroup mixerGroup; // 混音器轨道

    private void Start()
    {
        scaleOfCube = transform.localScale.x;
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position; // 存储初始位置
        audioSource = GetComponent<AudioSource>(); // 获取音效组件

        // 确保 audioSource 和 mixerGroup 不为空
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (mixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = mixerGroup; // 设置混音器轨道
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (isFlipping)
        {
            ExecuteFlip();
        }

        // 检测立方体的 Y 值
        if (transform.position.y <= fallThreshold)
        {
            // 回到初始位置
            transform.position = initialPosition;

            // 播放音效
            if (audioSource != null && fallSound != null)
            {
                audioSource.PlayOneShot(fallSound);
            }
        }
    }

    private void ExecuteFlip()
    {
        if (isFlipping)
        {
            float angleToRotate = RotateSpeed * Time.deltaTime;
            currentAngle += angleToRotate;

            transform.RotateAround(pivotPoint, flipAxis, angleToRotate);

            if (currentAngle >= 90)
            {
                isFlipping = false;
                currentAngle = 0;

                // 调整立方体的位置以适应地面
                Vector3 position = transform.position;
                position.x = Mathf.Round(position.x * 10) / 10;
                position.y = Mathf.Round(position.y * 10) / 10;
                position.z = Mathf.Round(position.z * 10) / 10;
                transform.position = position;

                // 确保立方体的旋转角度是 90 度的倍数
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.x = Mathf.Round(eulerAngles.x / 90) * 90;
                eulerAngles.y = Mathf.Round(eulerAngles.y / 90) * 90;
                eulerAngles.z = Mathf.Round(eulerAngles.z / 90) * 90;
                transform.eulerAngles = eulerAngles;
            }
        }
    }

    private void HandleInput()
    {
        if (isFlipping) return;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input.magnitude < 0.1f) return;

        flipAxis = -Vector3.Cross(input, Vector3.up);
        pivotPoint = transform.position + 0.5f * scaleOfCube * input.normalized;
        pivotPoint.y -= 0.5f * scaleOfCube;

        isFlipping = true;
    }
}
