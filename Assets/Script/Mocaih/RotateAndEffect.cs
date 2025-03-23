using UnityEngine;
using UnityEngine.Audio;

public class Rotate : MonoBehaviour
{
    public const float BASE_ACCELERATION = 40f; // 基础加速度
    private const float ACCELERATION_INCREMENT = 10f; // 每次点击增加的加速度
    private const float DECELERATION_MULTIPLIER = 0.1f; // 加速度衰减倍数
    private const float MAX_ROTATION_SPEED = 8000f; // 最大旋转速度
    private const float MIN_ROTATION_SPEED = 100f; // 最小旋转速度
    private const float ACCELERATION_DECAY_START_SPEED = 5000f; // 开始衰减加速度的旋转速度
    private const float MIN_PARTICLE_SIZE = 0.03f; // 最小粒子大小
    private const float MAX_PARTICLE_SIZE = 0.1f; // 最大粒子大小
    public int MAX_CLICK_COUNT = 20; // 达到最大速度所需的点击次数
    public const float MAX_SPEED_DURATION = 5f; // 保持最大速度的时间

    public ParticleSystem smokeEffect; // 粒子系统

    private float currentRotationSpeed = 0f; // 当前旋转速度
    private float currentAcceleration = 0f; // 当前加速度
    private float lastClickTime = 0f; // 上次点击时间
    private int clickCount = 0; // 点击次数
    private float maxSpeedStartTime = 0f; // 达到最大速度的开始时间
    private ParticleSystem.EmissionModule smokeEmission; // 粒子系统的发射模块
    private ParticleSystem.MainModule smokeMain; // 粒子系统的主模块
    private MeshRenderer _meshRenderer; // MeshRenderer 组件

    void Start()
    {
        InitializeParticleSystem();
        currentAcceleration = BASE_ACCELERATION; // 初始化当前加速度为基本加速度
        _meshRenderer = GetComponent<MeshRenderer>(); // 初始化 MeshRenderer 组件
    }

    void Update()
    {
        float currentTime = Time.time;

        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = currentTime; // 更新上次点击时间
            currentAcceleration += ACCELERATION_INCREMENT; // 增加加速度
            clickCount++; // 增加点击次数

            // 检查是否达到最大点击次数
            if (clickCount >= MAX_CLICK_COUNT)
            {
                currentRotationSpeed = MAX_ROTATION_SPEED; // 设置为最大速度
                maxSpeedStartTime = currentTime; // 记录达到最大速度的开始时间
                clickCount = 0; // 重置点击次数
            }
        }

        // 检查是否需要开始减速
        if (currentTime - lastClickTime > 3f)
        {
            currentAcceleration = Mathf.Lerp(currentAcceleration, 0f, DECELERATION_MULTIPLIER * Time.deltaTime);
        }

        // 更新旋转速度
        if (currentTime - maxSpeedStartTime > MAX_SPEED_DURATION)
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, MIN_ROTATION_SPEED, DECELERATION_MULTIPLIER * Time.deltaTime);
        }
        else
        {
            currentRotationSpeed += currentAcceleration * Time.deltaTime;
        }

        // 检查是否达到最大旋转速度并播放粒子系统
        if (currentRotationSpeed >= MAX_ROTATION_SPEED)
        {
            currentRotationSpeed = MAX_ROTATION_SPEED; // 保持最大速度
            currentAcceleration = 0f; // 停止加速

            // 启用粒子系统并维持其开始状态
            if (smokeEffect != null && !smokeEffect.isPlaying)
            {
                smokeEffect.Play();
            }
        }
        else if (currentRotationSpeed >= ACCELERATION_DECAY_START_SPEED)
        {
            // 当旋转速度达到5000转/秒时，开始衰减加速度
            currentAcceleration -= (currentRotationSpeed - ACCELERATION_DECAY_START_SPEED) * DECELERATION_MULTIPLIER * Time.deltaTime;
            currentAcceleration = Mathf.Max(currentAcceleration, 0f); // 确保加速度不会低于0
        }

        // 围绕Y轴旋转
        transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);

        // 根据当前旋转速度动态调整粒子系统的发射速率和3DStartSize
        if (smokeEffect != null)
        {
            smokeEmission.rateOverTime = Mathf.Lerp(smokeEmission.rateOverTime.constant, currentRotationSpeed / MAX_ROTATION_SPEED * 100f, Time.deltaTime);
            float startSize = Mathf.Lerp(MIN_PARTICLE_SIZE, MAX_PARTICLE_SIZE, currentRotationSpeed / MAX_ROTATION_SPEED); // 根据旋转速度调整粒子大小
            smokeMain.startSize = new ParticleSystem.MinMaxCurve(startSize); // 使用MinMaxCurve设置粒子大小范围
        }

        // 启用粒子系统当速度达到800转/秒
        if (currentRotationSpeed >= 800f && smokeEffect != null && !smokeEffect.isPlaying)
        {
            smokeEffect.Play();
        }

        // 根据旋转速度调整颜色
        AdjustColorBasedOnSpeed();
    }

    void AdjustColorBasedOnSpeed()
    {
        // 根据旋转速度设置颜色的饱和度
        Color baseColor = Color.red; // 基础颜色为红色
        Color targetColor;

        if (currentRotationSpeed < 500)
        {
            targetColor = Color.white; // 低速时为纯白色
        }
        else if (currentRotationSpeed < 4000)
        {
            float t = (currentRotationSpeed - 500) / 3500; // 计算插值因子
            targetColor = Color.Lerp(Color.white, baseColor, t * 0.3f); // 低速到中速时红色饱和度逐渐增加
        }
        else if (currentRotationSpeed < 7000)
        {
            float t = (currentRotationSpeed - 4000) / 3000; // 计算插值因子
            targetColor = Color.Lerp(Color.Lerp(Color.white, baseColor, 0.3f), baseColor, t); // 中速到高速时红色饱和度逐渐增加
        }
        else
        {
            targetColor = baseColor; // 高速时为纯红色
        }

        _meshRenderer.material.color = targetColor;
    }

    private void InitializeParticleSystem()
    {
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
            smokeEffect.Clear(); // 确保粒子系统被清除
            smokeEmission = smokeEffect.emission; // 获取发射模块
            smokeMain = smokeEffect.main; // 获取主模块
            smokeEmission.rateOverTime = 0f; // 初始化发射速率为0
        }
    }
}
