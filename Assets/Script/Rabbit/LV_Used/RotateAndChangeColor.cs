using UnityEngine;

public class RotateAndChangeColor : MonoBehaviour
{
    private Transform _transform;
    private MeshRenderer _meshRenderer;

    private Vector3 _previousRotation; // ��һ֡����ת�Ƕ�
    private float _rotationSpeed; // ��ת�ٶ�

    private Vector3 initialScale; // ��ʼ���ű���
    public float maxScaleMultiplier = 2.0f; // ������ű���

    void Start()
    {
        _transform = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        initialScale = _transform.localScale; // ��¼��ʼ���ű���
    }

    void Update()
    {
        // ���㵱ǰ֡����ת�Ƕ�
        Vector3 currentRotation = _transform.eulerAngles;

        // ������ת�ٶȣ�ÿ����ת�Ƕȣ�
        _rotationSpeed = Vector3.Distance(currentRotation, _previousRotation) / Time.deltaTime;

        // ������һ֡����ת�Ƕ�
        _previousRotation = currentRotation;

        // ������ת�ٶȵ�����ɫ
        AdjustColorBasedOnSpeed();

        // ������ת�ٶȵ������ű���
        AdjustScaleBasedOnSpeed();
    }

    void AdjustColorBasedOnSpeed()
    {
        // ������ת�ٶ�������ɫ�ı��Ͷ�
        Color baseColor = Color.red; // ������ɫΪ��ɫ
        Color targetColor;

        if (_rotationSpeed < 500)
        {
            targetColor = Color.white; // ����ʱΪ����ɫ
        }
        else if (_rotationSpeed < 4000)
        {
            float t = (_rotationSpeed - 500) / 3500; // �����ֵ����
            targetColor = Color.Lerp(Color.white, baseColor, t * 0.3f); // ���ٵ�����ʱ��ɫ���Ͷ�������
        }
        else if (_rotationSpeed < 7000)
        {
            float t = (_rotationSpeed - 4000) / 3000; // �����ֵ����
            targetColor = Color.Lerp(Color.Lerp(Color.white, baseColor, 0.3f), baseColor, t); // ���ٵ�����ʱ��ɫ���Ͷ�������
        }
        else
        {
            targetColor = baseColor; // ����ʱΪ����ɫ
        }

        _meshRenderer.material.color = targetColor;
    }

    void AdjustScaleBasedOnSpeed()
    {
        // ������ת�ٶ��������ű���
        float minSpeed = 500f;
        float maxSpeed = 7000f;
        float t = Mathf.Clamp((_rotationSpeed - minSpeed) / (maxSpeed - minSpeed), 0f, 1f); // �����ֵ����
        float scaleMultiplier = Mathf.Lerp(1f, maxScaleMultiplier, t); // �������ű���
        _transform.localScale = initialScale * scaleMultiplier; // �������ű���
    }
}
