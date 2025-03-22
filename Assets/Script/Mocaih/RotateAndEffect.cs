using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float baseAcceleration = 40f; // �������ٶ�
    public float accelerationIncrement = 10f; // ÿ�ε�����ӵļ��ٶ�
    public float decelerationMultiplier = 0.1f; // ���ٶ�˥������
    public float maxRotationSpeed = 99999f; // �����ת�ٶ�
    public float accelerationDecayStartSpeed = 8000f; // ��ʼ˥�����ٶȵ���ת�ٶ�
    public ParticleSystem smokeEffect; // ����ϵͳ

    private float currentRotationSpeed = 0f; // ��ǰ��ת�ٶ�
    private float currentAcceleration = 0f; // ��ǰ���ٶ�
    private float lastClickTime = 0f; // �ϴε��ʱ��

    void Start()
    {
        // ��ʼ����ǰ���ٶ�Ϊ�������ٶ�
        currentAcceleration = baseAcceleration;
    }

    void Update()
    {
        float currentTime = Time.time;

        // ��������
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = currentTime; // �����ϴε��ʱ��
            currentAcceleration += accelerationIncrement; // ���Ӽ��ٶ�
        }

        // ����Ƿ���Ҫ��ʼ����
        if (currentTime - lastClickTime > 3f)
        {
            currentAcceleration -= decelerationMultiplier * Time.deltaTime;
            currentAcceleration = Mathf.Max(currentAcceleration, 0f); // ȷ�����ٶȲ������0
        }

        // ������ת�ٶ�
        currentRotationSpeed += currentAcceleration * Time.deltaTime;

        // ����Ƿ�ﵽ�����ת�ٶȲ���������ϵͳ
        if (currentRotationSpeed >= maxRotationSpeed)
        {
            currentRotationSpeed = maxRotationSpeed; // ��������ٶ�
            currentAcceleration = 0f; // ֹͣ����
        }
        else if (currentRotationSpeed >= accelerationDecayStartSpeed)
        {
            // ����ת�ٶȴﵽ8000ת/��ʱ����ʼ˥�����ٶ�
            currentAcceleration -= (currentRotationSpeed - accelerationDecayStartSpeed) * decelerationMultiplier * Time.deltaTime;
            currentAcceleration = Mathf.Max(currentAcceleration, 0f); // ȷ�����ٶȲ������0
        }

        // Χ��Y����ת
        transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);

        // ����Ƿ�ﵽ5000ת/�벢��������ϵͳ
        if (currentRotationSpeed >= 5000f && smokeEffect != null)
        {
            smokeEffect.Play();
        }
    }
}
