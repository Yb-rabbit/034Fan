using UnityEngine;

public class BlockController : MonoBehaviour
{
    public float fallSpeed = 1f; // 下落速度，单位/秒
    public float deletionHeight = -5f; // 方块被删除的最低高度
    public Transform spawnPoint; // 生成方块的位置
    public GameObject blockPrefab; // 方块的预制体
    public float spawnDelay = 2f; // 重新生成方块的延迟时间

    private bool isBlockActive = true; // 用于跟踪方块是否处于活动状态

    void Update()
    {
        if (isBlockActive)
        {
            // 使方块下落
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 停止下落
        fallSpeed = 0;
    }

    private void Start()
    {
        Invoke("SpawnBlock", spawnDelay); // 游戏开始时延迟生成方块
    }

    private void SpawnBlock()
    {
        if (!isBlockActive)
        {
            // 在指定位置生成方块
            Instantiate(blockPrefab, spawnPoint.position, Quaternion.identity);
            isBlockActive = true;
        }
    }

    void CheckBlockPosition()
    {
        // 检查方块是否超出指定高度
        if (transform.position.y < deletionHeight)
        {
            Destroy(gameObject); // 删除方块
            isBlockActive = false; // 标记方块为非活动状态
            Invoke("SpawnBlock", spawnDelay); // 延迟一定时间后重新生成方块
        }
    }
}
