using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 checkpointPosition; // �Զ������λ��

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ������ҵı�ǩ��"Player"
        {
            if (other.TryGetComponent(out Movement_B movementB))
            {
                movementB.SetCheckpoint(checkpointPosition);
                movementB.ResetRotation(); // ������ҵ���ת����
                Debug.Log("Player entered checkpoint and rotation reset.");
            }
            else if (other.TryGetComponent(out Movement_R movementR))
            {
                movementR.ResetToCheckpoint(); // ǿ��������������
                Debug.Log("Player entered checkpoint and all properties reset.");
            }
            else
            {
                Debug.LogWarning("Player does not have a Movement_B or Movement_R component.");
            }
        }
    }
}

