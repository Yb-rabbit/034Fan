using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridSizeX = 10; // X方向格子数量
    public int gridSizeY = 10; // Y方向格子数量
    public float cellSize = 1.0f; // 每个格子的大小
    public Vector3 normal = Vector3.up; // 网格的法线方向
    public Vector3 reference = Vector3.forward; // 网格的参考方向

    private Matrix4x4 localToWorldMatrix; // 局部坐标到世界坐标的矩阵
    private Matrix4x4 worldToLocalMatrix; // 世界坐标到局部坐标的矩阵

    void Start()
    {
        // 计算局部坐标系
        Vector3 xAxis = Vector3.Cross(normal, reference).normalized;
        Vector3 yAxis = Vector3.Cross(normal, xAxis).normalized;
        Vector3 zAxis = normal.normalized;

        // 构建矩阵
        localToWorldMatrix = Matrix4x4.Rotate(Quaternion.LookRotation(zAxis, yAxis));
        worldToLocalMatrix = localToWorldMatrix.inverse;
    }

    // 根据格子索引计算世界坐标
    public Vector3 GetCellWorldPosition(int x, int y)
    {
        Vector3 localPosition = new Vector3(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2, 0);
        return localToWorldMatrix.MultiplyPoint(localPosition);
    }

    // 根据世界坐标计算格子索引
    public (int, int) GetCellIndexByWorldPosition(Vector3 worldPos)
    {
        Vector3 localPosition = worldToLocalMatrix.MultiplyPoint(worldPos);
        return ((int)(localPosition.x / cellSize), (int)(localPosition.y / cellSize));
    }

    // 在指定格子上放置物体
    public void PlaceObjectAtCell(GameObject obj, int x, int y)
    {
        obj.transform.position = GetCellWorldPosition(x, y);
    }
}