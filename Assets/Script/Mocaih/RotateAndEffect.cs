using UnityEngine;
using UnityEngine.Audio;

public class Rotate : MonoBehaviour
{
    public const float BASE_ACCELERATION = 40f; // �������ٶ�
    private const float ACCELERATION_INCREMENT = 10f; // ÿ�ε�����ӵļ��ٶ�
    private const float DECELERATION_MULTIPLIER = 0.1f; // ���ٶ�˥������
    private const float MAX_ROTATION_SPEED = 8000f; // �����ת�ٶ�
    private const float MIN_ROTATION_SPEED = 100f; // ��С��ת�ٶ�
    private const float ACCELERATION_DECAY_START_SPEED = 5000f; // ��ʼ˥�����ٶȵ���ת�ٶ�
    private const float MIN_PARTICLE_SIZE = 0.03f; // ��С���Ӵ�С
    private const float MAX_PARTICLE_SIZE = 0.1f; // ������Ӵ�С
    public int MAX_CLICK_COUNT = 20; // �ﵽ����ٶ�����ĵ������
    public const float MAX_SPEED_DURATION = 5f; // ��������ٶȵ�ʱ��

    public ParticleSystem smokeEffect; // ����ϵͳ

    private float currentRotationSpeed = 0f; // ��ǰ��ת�ٶ�
    private float currentAcceleration = 0f; // ��ǰ���ٶ�
    private float lastClickTime = 0f; // �ϴε��ʱ��
    private int clickCount = 0; // �������
    private float maxSpeedStartTime = 0f; // �ﵽ����ٶȵĿ�ʼʱ��
    private ParticleSystem.EmissionModule smokeEmission; // ����ϵͳ�ķ���ģ��
    private ParticleSystem.MainModule smokeMain; // ����ϵͳ����ģ��
    private MeshRenderer _meshRenderer; // MeshRenderer ���

    void Start()
    {
        InitializeParticleSystem();
        currentAcceleration = BASE_ACCELERATION; // ��ʼ����ǰ���ٶ�Ϊ�������ٶ�
        _meshRenderer = GetComponent<MeshRenderer>(); // ��ʼ�� MeshRenderer ���
    }

    void Update()
    {
        float currentTime = Time.time;

        // ��������
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = currentTime; // �����ϴε��ʱ��
            currentAcceleration += ACCELERATION_INCREMENT; // ���Ӽ��ٶ�
            clickCount++; // ���ӵ������

            // ����Ƿ�ﵽ���������
            if (clickCount >= MAX_CLICK_COUNT)
            {
                currentRotationSpeed = MAX_ROTATION_SPEED; // ����Ϊ����ٶ�
                maxSpeedStartTime = currentTime; // ��¼�ﵽ����ٶȵĿ�ʼʱ��
                clickCount = 0; // ���õ������
            }
        }

        // ����Ƿ���Ҫ��ʼ����
        if (currentTime - lastClickTime > 3f)
        {
            currentAcceleration = Mathf.Lerp(currentAcceleration, 0f, DECELERATION_MULTIPLIER * Time.deltaTime);
        }

        // ������ת�ٶ�
        if (currentTime - maxSpeedStartTime > MAX_SPEED_DURATION)
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, MIN_ROTATION_SPEED, DECELERATION_MULTIPLIER * Time.deltaTime);
        }
        else
        {
            currentRotationSpeed += currentAcceleration * Time.deltaTime;
        }

        // ����Ƿ�ﵽ�����ת�ٶȲ���������ϵͳ
        if (currentRotationSpeed >= MAX_ROTATION_SPEED)
        {
            currentRotationSpeed = MAX_ROTATION_SPEED; // ��������ٶ�
            currentAcceleration = 0f; // ֹͣ����

            // ��������ϵͳ��ά���俪ʼ״̬
            if (smokeEffect != null && !smokeEffect.isPlaying)
            {
                smokeEffect.Play();
            }
        }
        else if (currentRotationSpeed >= ACCELERATION_DECAY_START_SPEED)
        {
            // ����ת�ٶȴﵽ5000ת/��ʱ����ʼ˥�����ٶ�
            currentAcceleration -= (currentRotationSpeed - ACCELERATION_DECAY_START_SPEED) * DECELERATION_MULTIPLIER * Time.deltaTime;
            currentAcceleration = Mathf.Max(currentAcceleration, 0f); // ȷ�����ٶȲ������0
        }

        // Χ��Y����ת
        transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);

        // ���ݵ�ǰ��ת�ٶȶ�̬��������ϵͳ�ķ������ʺ�3DStartSize
        if (smokeEffect != null)
        {
            smokeEmission.rateOverTime = Mathf.Lerp(smokeEmission.rateOverTime.constant, currentRotationSpeed / MAX_ROTATION_SPEED * 100f, Time.deltaTime);
            float startSize = Mathf.Lerp(MIN_PARTICLE_SIZE, MAX_PARTICLE_SIZE, currentRotationSpeed / MAX_ROTATION_SPEED); // ������ת�ٶȵ������Ӵ�С
            smokeMain.startSize = new ParticleSystem.MinMaxCurve(startSize); // ʹ��MinMaxCurve�������Ӵ�С��Χ
        }

        // ��������ϵͳ���ٶȴﵽ800ת/��
        if (currentRotationSpeed >= 800f && smokeEffect != null && !smokeEffect.isPlaying)
        {
            smokeEffect.Play();
        }

        // ������ת�ٶȵ�����ɫ
        AdjustColorBasedOnSpeed();
    }

    void AdjustColorBasedOnSpeed()
    {
        // ������ת�ٶ�������ɫ�ı��Ͷ�
        Color baseColor = Color.red; // ������ɫΪ��ɫ
        Color targetColor;

        if (currentRotationSpeed < 500)
        {
            targetColor = Color.white; // ����ʱΪ����ɫ
        }
        else if (currentRotationSpeed < 4000)
        {
            float t = (currentRotationSpeed - 500) / 3500; // �����ֵ����
            targetColor = Color.Lerp(Color.white, baseColor, t * 0.3f); // ���ٵ�����ʱ��ɫ���Ͷ�������
        }
        else if (currentRotationSpeed < 7000)
        {
            float t = (currentRotationSpeed - 4000) / 3000; // �����ֵ����
            targetColor = Color.Lerp(Color.Lerp(Color.white, baseColor, 0.3f), baseColor, t); // ���ٵ�����ʱ��ɫ���Ͷ�������
        }
        else
        {
            targetColor = baseColor; // ����ʱΪ����ɫ
        }

        _meshRenderer.material.color = targetColor;
    }

    private void InitializeParticleSystem()
    {
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
            smokeEffect.Clear(); // ȷ������ϵͳ�����
            smokeEmission = smokeEffect.emission; // ��ȡ����ģ��
            smokeMain = smokeEffect.main; // ��ȡ��ģ��
            smokeEmission.rateOverTime = 0f; // ��ʼ����������Ϊ0
        }
    }
}
