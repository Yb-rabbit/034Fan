using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GridManager gridManager; // ���� GridManager
    public GameObject objectPrefab; // Ҫ���õ����� Prefab
    public int maxObjectCount = 10; // �����������

    private int currentObjectCount = 0; // ��ǰ�����ɵ���������

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ����������
        {
            if (currentObjectCount >= maxObjectCount)
            {
                Debug.Log("�Ѵﵽ�����������");
                return; // ����Ѵﵽ����������������������µ�����
            }

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
                    currentObjectCount++; // ���������ɵ���������
                }
            }
        }
    }
}
