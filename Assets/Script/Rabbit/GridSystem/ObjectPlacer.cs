using UnityEngine;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public GridManager gridManager; // ���� GridManager
    public GameObject objectPrefab; // Ҫ���õ����� Prefab
    public int maxObjectCount = 10; // �����������
    public float maxExistenceTime = 10f; // ������ʱ�䣨�룩
    public float shrinkDuration = 2f; // ��С����ʱ�䣨�룩

    private int currentObjectCount = 0; // ��ǰ�����ɵ���������
    private Dictionary<GameObject, float> placedObjects = new Dictionary<GameObject, float>(); // �ѷ��õ����弰������ʱ��

    void Update()
    {
        float currentTime = Time.time;

        // ��鲢������������ʱ�������
        CheckAndHandleObjects(currentTime);

        // ��������
        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject(currentTime);
        }
    }

    private void CheckAndHandleObjects(float currentTime)
    {
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var kvp in placedObjects)
        {
            float elapsedTime = currentTime - kvp.Value;
            if (elapsedTime > maxExistenceTime)
            {
                objectsToRemove.Add(kvp.Key);
            }
            else if (elapsedTime > maxExistenceTime - shrinkDuration)
            {
                // ƽ����С����
                float shrinkFactor = 1f - (elapsedTime - (maxExistenceTime - shrinkDuration)) / shrinkDuration;
                kvp.Key.transform.localScale = Vector3.one * shrinkFactor;
            }
        }

        foreach (var obj in objectsToRemove)
        {
            Destroy(obj);
            placedObjects.Remove(obj);
            currentObjectCount--; // ���������ɵ���������
        }
    }

    private void PlaceObject(float currentTime)
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
                GameObject newObject = Instantiate(objectPrefab);
                gridManager.PlaceObjectAtCell(newObject, cellIndex.Item1, cellIndex.Item2);
                placedObjects.Add(newObject, currentTime); // ��ӵ��ѷ��õ����弰������ʱ��
                currentObjectCount++; // ���������ɵ���������
            }
        }
    }
}
