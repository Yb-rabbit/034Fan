using UnityEngine;

public class CubeSwitcher : MonoBehaviour
{
    public GameObject[] cubePrefabs; // ����������Ԥ���������
    private GameObject currentCube; // ��ǰ�����������
    private int currentCubeIndex = 0; // ��ǰ�����������
    public float yOffset = 1.0f; // �¾�������֮���Y�����

    void Start()
    {
        // ��ʼ��ʱ���ص�һ��������
        currentCube = Instantiate(cubePrefabs[currentCubeIndex], transform.position, Quaternion.identity);
        currentCube.transform.SetParent(transform);
    }

    void Update()
    {
        // ���� T ��ʱ�л�������
        if (Input.GetKeyDown(KeyCode.T))
        {
            NextCube();
        }
    }

    // �л�����һ��������ķ���
    public void NextCube()
    {
        // �л�����һ������������
        int nextIndex = (currentCubeIndex + 1) % cubePrefabs.Length;
        SwitchCube(nextIndex);
    }

    // �л���ָ��������������ķ���
    public void SwitchCube(int index)
    {
        if (index >= 0 && index < cubePrefabs.Length)
        {
            // ��ȡ��ǰ�������λ��
            Vector3 currentPosition = currentCube.transform.position;

            // ���ٵ�ǰ������
            Destroy(currentCube);

            // ʵ�����µ�������
            currentCube = Instantiate(cubePrefabs[index], currentPosition, Quaternion.identity);
            currentCube.transform.SetParent(transform);

            // ���µ�ǰ����������
            currentCubeIndex = index;

            // �����������λ�õ�����ԭ���������λ�ã�����Y��������һ���ľ���
            currentCube.transform.position = new Vector3(currentPosition.x, currentPosition.y + yOffset, currentPosition.z);

            // ��ѡ�������л���Ч����Ч
        }
    }
}
