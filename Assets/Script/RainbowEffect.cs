using UnityEngine;

public class RainbowEffect : MonoBehaviour
{
    public Material mat; // ָ������
    private float time;

    void Update()
    {
        time += Time.deltaTime;
        mat.color = CalculateRainbowColor(time);
    }

    /// <summary>
    /// ����ʺ���ɫ
    /// </summary>
    /// <param name="time">ʱ�����</param>
    /// <returns>���ؼ�������ɫ</returns>
    private Color CalculateRainbowColor(float time)
    {
        // ʹ�� Mathf.PingPong ����ʵ����ɫ��ѭ������
        float r = Mathf.PingPong(time, 1);
        float g = Mathf.PingPong(time + 0.33f, 1);
        float b = Mathf.PingPong(time + 0.66f, 1);
        return new Color(r, g, b, 1.0f); // ������ɫ
    }
}
