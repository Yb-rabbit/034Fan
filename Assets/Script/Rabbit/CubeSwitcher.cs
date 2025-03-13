using UnityEngine;

public class CubeSwitcher : MonoBehaviour
{
    public GameObject[] cubePrefabs; // 所有立方体预制体的数组
    private GameObject currentCube; // 当前激活的立方体
    private int currentCubeIndex = 0; // 当前立方体的索引
    public float yOffset = 1.0f; // 新旧立方体之间的Y轴距离

    void Start()
    {
        // 初始化时加载第一个立方体
        currentCube = Instantiate(cubePrefabs[currentCubeIndex], transform.position, Quaternion.identity);
        currentCube.transform.SetParent(transform);
    }

    void Update()
    {
        // 按下 T 键时切换立方体
        if (Input.GetKeyDown(KeyCode.T))
        {
            NextCube();
        }
    }

    // 切换到下一个立方体的方法
    public void NextCube()
    {
        // 切换到下一个立方体索引
        int nextIndex = (currentCubeIndex + 1) % cubePrefabs.Length;
        SwitchCube(nextIndex);
    }

    // 切换到指定索引的立方体的方法
    public void SwitchCube(int index)
    {
        if (index >= 0 && index < cubePrefabs.Length)
        {
            // 获取当前立方体的位置
            Vector3 currentPosition = currentCube.transform.position;

            // 销毁当前立方体
            Destroy(currentCube);

            // 实例化新的立方体
            currentCube = Instantiate(cubePrefabs[index], currentPosition, Quaternion.identity);
            currentCube.transform.SetParent(transform);

            // 更新当前立方体索引
            currentCubeIndex = index;

            // 将新立方体的位置调整到原来立方体的位置，并在Y轴上增加一定的距离
            currentCube.transform.position = new Vector3(currentPosition.x, currentPosition.y + yOffset, currentPosition.z);

            // 可选：播放切换音效或特效
        }
    }
}
