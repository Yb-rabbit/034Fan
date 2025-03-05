using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 checkpointPosition; // 自定义检查点位置

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 假设玩家的标签是"Player"
        {
            if (other.TryGetComponent(out Movement_B movement))
            {
                movement.SetCheckpoint(checkpointPosition);
                Debug.Log("Player entered checkpoint.");
            }
            else
            {
                Debug.LogWarning("Player does not have a Movement_B component.");
            }
        }
    }
}
