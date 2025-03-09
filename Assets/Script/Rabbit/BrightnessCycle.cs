using UnityEngine;
using UnityEngine.UI; // ���ʹ��Text���
using TMPro; // ���ʹ��TextMeshPro���

public class BrightnessCycle : MonoBehaviour
{
    public Text textComponent; // ���ʹ��Text���
    // public TextMeshProUGUI textComponent; // ���ʹ��TextMeshPro���

    public float duration = 2.0f; // ���ȱ仯������ʱ�䣨�룩
    private float elapsedTime = 0.0f;

    private Color baseColor; // ������ɫ
    public float minBrightness = 0.5f; // ��С����
    public float maxBrightness = 1.0f; // �������

    private int baseFontSize; // ���������С
    public int minFontSize = 20; // ��С�����С
    public int maxFontSize = 30; // ��������С

    public bool keepCurrentFontSize = false; // �Ƿ񱣳ֵ�ǰ�����С

    void Start()
    {
        baseColor = textComponent.color; // ��ȡ��ʼ��ɫ
        baseFontSize = textComponent.fontSize; // ��ȡ��ʼ�����С
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // ���㵱ǰ����
        float brightness = Mathf.Sin((elapsedTime / duration) * Mathf.PI * 2) * 0.5f + 0.5f;

        // ������ӳ�䵽��С���������֮��
        brightness = Mathf.Lerp(minBrightness, maxBrightness, brightness);

        // ������ɫ
        textComponent.color = new Color(
            baseColor.r * brightness,
            baseColor.g * brightness,
            baseColor.b * brightness,
            baseColor.a // ����͸���Ȳ���
        );

        // ��������ֵ�ǰ�����С������������С
        if (!keepCurrentFontSize)
        {
            // ���㵱ǰ�����С
            float fontSize = Mathf.Sin((elapsedTime / duration) * Mathf.PI * 2) * 0.5f + 0.5f;

            // �������Сӳ�䵽��С����������С֮��
            fontSize = Mathf.Lerp(minFontSize, maxFontSize, fontSize);

            // ���������С
            textComponent.fontSize = Mathf.RoundToInt(fontSize);
        }
    }
}
