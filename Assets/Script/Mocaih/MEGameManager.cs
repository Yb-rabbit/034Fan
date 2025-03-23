using UnityEngine;
using System.Collections;

public class MEGameManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // 存储所有方块预制体的数组
    public Transform spawnPoint; // 生成点位置
    private float nextSpawnTime = 0f; // 控制方块生成的时间间隔

    public float maxLifeTime = 5f; // 方块最大存在时间
    public float shrinkTime = 1f; // 方块消失前的缩小时间

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
        StartCoroutine(BlockLifeCycle(block));
    }

    IEnumerator BlockLifeCycle(GameObject block)
    {
        yield return new WaitForSeconds(maxLifeTime - shrinkTime);

        Vector3 originalScale = block.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float shrinkDuration = shrinkTime;

        for (float t = 0; t < shrinkDuration; t += Time.deltaTime)
        {
            block.transform.localScale = Vector3.Lerp(originalScale, targetScale, t / shrinkDuration);
            yield return null;
        }

        Destroy(block);
    }
}
