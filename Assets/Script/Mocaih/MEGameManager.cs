using UnityEngine;
using System.Collections;

public class MEGameManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // �洢���з���Ԥ���������
    public Transform spawnPoint; // ���ɵ�λ��
    private float nextSpawnTime = 0f; // ���Ʒ������ɵ�ʱ����

    public float maxLifeTime = 5f; // ����������ʱ��
    public float shrinkTime = 1f; // ������ʧǰ����Сʱ��

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnBlock();
            nextSpawnTime = Time.time + 3f; // ÿ������һ������
        }
    }

    void SpawnBlock()
    {
        if (blockPrefabs.Length == 0)
        {
            Debug.LogError("û�з���Ԥ���壡");
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
