using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float fallSpeed = 1f; // �����ٶȣ���λ/��
    public float deletionHeight = -5f; // ���鱻ɾ������͸߶�
    public Transform spawnPoint; // ���ɷ����λ��
    public GameObject blockPrefab; // �����Ԥ����
    public float spawnDelay = 2f; // �������ɷ�����ӳ�ʱ��

    private bool isBlockActive = true; // ���ڸ��ٷ����Ƿ��ڻ״̬

    void Update()
    {
        if (isBlockActive)
        {
            // ʹ��������
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ֹͣ����
        fallSpeed = 0;
    }

    private void Start()
    {
        Invoke("SpawnBlock", spawnDelay); // ��Ϸ��ʼʱ�ӳ����ɷ���
    }

    private void SpawnBlock()
    {
        if (!isBlockActive)
        {
            // ��ָ��λ�����ɷ���
            Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);
            isBlockActive = true;
        }
    }

    void CheckBlockPosition()
    {
        // ��鷽���Ƿ񳬳�ָ���߶�
        if (transform.position.y < deletionHeight)
        {
            Destroy(gameObject); // ɾ������
            isBlockActive = false; // ��Ƿ���Ϊ�ǻ״̬
            Invoke("SpawnBlock", spawnDelay); // �ӳ�һ��ʱ����������ɷ���
        }
    }
}
