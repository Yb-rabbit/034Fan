using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Vector3 checkpointPosition; // �Զ������λ��

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ������ҵı�ǩ��"Player"
        {
            other.GetComponent<Movement_B>().SetCheckpoint(checkpointPosition);
        }
    }
}

