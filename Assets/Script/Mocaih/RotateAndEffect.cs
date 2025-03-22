using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float baseAcceleration = 40f; // 基础加速度
    public float accelerationIncrement = 10f; // 每次点击增加的加速度
    public float decelerationMultiplier = 0.1f; // 加速度衰减倍数
    public float maxRotationSpeed = 99999f; // 最大旋转速度
    public float accelerationDecayStartSpeed = 8000f; // 开始衰减加速度的旋转速度
    public ParticleSystem smokeEffect; // 粒子系统

    private float currentRotationSpeed = 0f; // 当前旋转速度
    private float currentAcceleration = 0f; // 当前加速度
    private float lastClickTime = 0f; // 上次点击时间

    void Start()
    {
        // 初始化当前加速度为基本加速度
        currentAcceleration = baseAcceleration;
    }

    void Update()
    {
        float currentTime = Time.time;

        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = currentTime; // 更新上次点击时间
            currentAcceleration += accelerationIncrement; // 增加加速度
        }

        // 检查是否需要开始减速
        if (currentTime - lastClickTime > 3f)
        {
            currentAcceleration -= decelerationMultiplier * Time.deltaTime;
            currentAcceleration = Mathf.Max(currentAcceleration, 0f); // 确保加速度不会低于0
        }

        // 更新旋转速度
        currentRotationSpeed += currentAcceleration * Time.deltaTime;

        // 检查是否达到最大旋转速度并播放粒子系统
        if (currentRotationSpeed >= maxRotationSpeed)
        {
            currentRotationSpeed = maxRotationSpeed; // 保持最大速度
            currentAcceleration = 0f; // 停止加速
        }
        else if (currentRotationSpeed >= accelerationDecayStartSpeed)
        {
            // 当旋转速度达到8000转/秒时，开始衰减加速度
            currentAcceleration -= (currentRotationSpeed - accelerationDecayStartSpeed) * decelerationMultiplier * Time.deltaTime;
            currentAcceleration = Mathf.Max(currentAcceleration, 0f); // 确保加速度不会低于0
        }

        // 围绕Y轴旋转
        transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);

        // 检查是否达到5000转/秒并播放粒子系统
        if (currentRotationSpeed >= 5000f && smokeEffect != null)
        {
            smokeEffect.Play();
        }
    }
}
