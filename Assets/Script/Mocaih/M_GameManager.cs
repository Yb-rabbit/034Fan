using UnityEngine;

public class M_GameManager : MonoBehaviour
{
    public GameObject[] blockPrefabs; // �洢���з���Ԥ���������
    public Transform spawnPoint; // ���ɵ�λ��
    private float nextSpawnTime = 0f; // ���Ʒ������ɵ�ʱ����

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
    }
}
