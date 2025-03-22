using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GridManager gridManager; // 引用 GridManager
    public GameObject objectPrefab; // 要放置的物体 Prefab
    public int maxObjectCount = 10; // 最大生成数量

    private int currentObjectCount = 0; // 当前已生成的物体数量

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 按下鼠标左键
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
                    gridManager.PlaceObjectAtCell(Instantiate(objectPrefab), cellIndex.Item1, cellIndex.Item2);
                    currentObjectCount++; // 增加已生成的物体数量
                }
            }
        }
    }
}
