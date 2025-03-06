using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 checkpointPosition; // 自定义检查点位置

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 假设玩家的标签是"Player"
        {
            if (other.TryGetComponent(out Movement_B movementB))
            {
                movementB.SetCheckpoint(checkpointPosition);
                movementB.ResetRotation(); // 重置玩家的旋转属性
                Debug.Log("Player entered checkpoint and rotation reset.");
            }
            else if (other.TryGetComponent(out Movement_R movementR))
            {
                movementR.ResetRotation(); // 重置玩家的旋转属性
                Debug.Log("Player entered checkpoint and rotation reset.");
            }
            else
            {
                Debug.LogWarning("Player does not have a Movement_B or Movement_R component.");
            }
        }
    }
}
