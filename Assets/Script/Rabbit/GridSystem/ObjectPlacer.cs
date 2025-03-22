using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GridManager gridManager; // 引用 GridManager
    public GameObject objectPrefab; // 要放置的物体 Prefab

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 按下鼠标左键
        {
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
                }
            }
        }
    }
}
