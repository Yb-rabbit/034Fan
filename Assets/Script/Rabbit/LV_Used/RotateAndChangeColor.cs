using UnityEngine;

public class RotateAndChangeColor : MonoBehaviour
{
    private Transform _transform;
    private MeshRenderer _meshRenderer;

    private Vector3 _previousRotation; // 上一帧的旋转角度
    private float _rotationSpeed; // 旋转速度

    private Vector3 initialScale; // 初始缩放比例
    public float maxScaleMultiplier = 2.0f; // 最大缩放倍数

    void Start()
    {
        _transform = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        initialScale = _transform.localScale; // 记录初始缩放比例
    }

    void Update()
    {
        // 计算当前帧的旋转角度
        Vector3 currentRotation = _transform.eulerAngles;

        // 计算旋转速度（每秒旋转角度）
        _rotationSpeed = Vector3.Distance(currentRotation, _previousRotation) / Time.deltaTime;

        // 更新上一帧的旋转角度
        _previousRotation = currentRotation;

        // 根据旋转速度调整颜色
        AdjustColorBasedOnSpeed();

        // 根据旋转速度调整缩放比例
        AdjustScaleBasedOnSpeed();
    }

    void AdjustColorBasedOnSpeed()
    {
        // 根据旋转速度设置颜色的饱和度
        Color baseColor = Color.red; // 基础颜色为红色
        Color targetColor;

        if (_rotationSpeed < 500)
        {
            targetColor = Color.white; // 低速时为纯白色
        }
        else if (_rotationSpeed < 4000)
        {
            float t = (_rotationSpeed - 500) / 3500; // 计算插值因子
            targetColor = Color.Lerp(Color.white, baseColor, t * 0.3f); // 低速到中速时红色饱和度逐渐增加
        }
        else if (_rotationSpeed < 7000)
        {
            float t = (_rotationSpeed - 4000) / 3000; // 计算插值因子
            targetColor = Color.Lerp(Color.Lerp(Color.white, baseColor, 0.3f), baseColor, t); // 中速到高速时红色饱和度逐渐增加
        }
        else
        {
            targetColor = baseColor; // 高速时为纯红色
        }

        _meshRenderer.material.color = targetColor;
    }

    void AdjustScaleBasedOnSpeed()
    {
        // 根据旋转速度设置缩放比例
        float minSpeed = 500f;
        float maxSpeed = 7000f;
        float t = Mathf.Clamp((_rotationSpeed - minSpeed) / (maxSpeed - minSpeed), 0f, 1f); // 计算插值因子
        float scaleMultiplier = Mathf.Lerp(1f, maxScaleMultiplier, t); // 计算缩放倍数
        _transform.localScale = initialScale * scaleMultiplier; // 设置缩放比例
    }
}
