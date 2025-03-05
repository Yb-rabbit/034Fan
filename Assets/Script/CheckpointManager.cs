using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 checkpointPosition; // �Զ������λ��

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ������ҵı�ǩ��"Player"
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
