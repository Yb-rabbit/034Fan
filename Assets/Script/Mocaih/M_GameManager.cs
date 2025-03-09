using UnityEngine;

public class M_GameManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // 存储所有方块预制体的数组
    public Transform spawnPoint; // 生成点位置
    private float nextSpawnTime = 0f; // 控制方块生成的时间间隔

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnBlock();
            nextSpawnTime = Time.time + 3f; // 每秒生成一个方块
        }
    }

    void SpawnBlock()
    {
        if (blockPrefabs.Length == 0)
        {
            Debug.LogError("没有方块预制体！");
            return;
        }

        int index = Random.Range(0, blockPrefabs.Length);
        GameObject block = Instantiate(blockPrefabs[index], spawnPoint.position, Quaternion.identity);
    }
}
