using UnityEngine;
using System.Collections.Generic;

public class ObjectPlacer : MonoBehaviour
{
    public GridManager gridManager; // 引用 GridManager
    public GameObject objectPrefab; // 要放置的物体 Prefab
    public int maxObjectCount = 10; // 最大生成数量
    public float maxExistenceTime = 10f; // 最大存在时间（秒）
    public float shrinkDuration = 2f; // 缩小持续时间（秒）

    private int currentObjectCount = 0; // 当前已生成的物体数量
    private Dictionary<GameObject, float> placedObjects = new Dictionary<GameObject, float>(); // 已放置的物体及其生成时间

    void Update()
    {
        float currentTime = Time.time;

        // 检查并处理超过最大存在时间的物体
        CheckAndHandleObjects(currentTime);

        // 检测鼠标点击
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
                // 平滑缩小物体
                float shrinkFactor = 1f - (elapsedTime - (maxExistenceTime - shrinkDuration)) / shrinkDuration;
                kvp.Key.transform.localScale = Vector3.one * shrinkFactor;
            }
        }

        foreach (var obj in objectsToRemove)
        {
            Destroy(obj);
            placedObjects.Remove(obj);
            currentObjectCount--; // 减少已生成的物体数量
        }
    }

    private void PlaceObject(float currentTime)
    {
        if (currentObjectCount >= maxObjectCount)
        {
            Debug.Log("已达到最大生成数量");
            return; // 如果已达到最大生成数量，则不再生成新的物体
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // 检测鼠标点击位置
        {
            if (hit.collider != null) // 确保碰撞到的是可见物体
            {
                // 获取鼠标点击位置所在的网格格子索引
                var cellIndex = gridManager.GetCellIndexByWorldPosition(hit.point);
                // 在该格子上放置物体
                GameObject newObject = Instantiate(objectPrefab);
                gridManager.PlaceObjectAtCell(newObject, cellIndex.Item1, cellIndex.Item2);
                placedObjects.Add(newObject, currentTime); // 添加到已放置的物体及其生成时间
                currentObjectCount++; // 增加已生成的物体数量
            }
        }
    }
}
