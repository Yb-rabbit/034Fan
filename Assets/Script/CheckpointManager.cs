using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector3 checkpointPosition; // 自定义检查点位置

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 假设玩家的标签是"Player"
        {
            other.GetComponent<Movement_B>().SetCheckpoint(checkpointPosition);
        }
    }
}

