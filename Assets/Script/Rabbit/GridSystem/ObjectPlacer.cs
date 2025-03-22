using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GridManager gridManager; // ���� GridManager
    public GameObject objectPrefab; // Ҫ���õ����� Prefab

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ����������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // ��������λ��
            {
                if (hit.collider != null) // ȷ����ײ�����ǿɼ�����
                {
                    // ��ȡ�����λ�����ڵ������������
                    var cellIndex = gridManager.GetCellIndexByWorldPosition(hit.point);
                    // �ڸø����Ϸ�������
                    gridManager.PlaceObjectAtCell(Instantiate(objectPrefab), cellIndex.Item1, cellIndex.Item2);
                }
            }
        }
    }
}
