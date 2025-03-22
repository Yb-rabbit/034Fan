using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridSizeX = 10; // X�����������
    public int gridSizeY = 10; // Y�����������
    public float cellSize = 1.0f; // ÿ�����ӵĴ�С
    public Vector3 normal = Vector3.up; // ����ķ��߷���
    public Vector3 reference = Vector3.forward; // ����Ĳο�����

    private Matrix4x4 localToWorldMatrix; // �ֲ����굽��������ľ���
    private Matrix4x4 worldToLocalMatrix; // �������굽�ֲ�����ľ���

    void Start()
    {
        // ����ֲ�����ϵ
        Vector3 xAxis = Vector3.Cross(normal, reference).normalized;
        Vector3 yAxis = Vector3.Cross(normal, xAxis).normalized;
        Vector3 zAxis = normal.normalized;

        // ��������
        localToWorldMatrix = Matrix4x4.Rotate(Quaternion.LookRotation(zAxis, yAxis));
        worldToLocalMatrix = localToWorldMatrix.inverse;
    }

    // ���ݸ�������������������
    public Vector3 GetCellWorldPosition(int x, int y)
    {
        Vector3 localPosition = new Vector3(x * cellSize + cellSize / 2, y * cellSize + cellSize / 2, 0);
        return localToWorldMatrix.MultiplyPoint(localPosition);
    }

    // ����������������������
    public (int, int) GetCellIndexByWorldPosition(Vector3 worldPos)
    {
        Vector3 localPosition = worldToLocalMatrix.MultiplyPoint(worldPos);
        return ((int)(localPosition.x / cellSize), (int)(localPosition.y / cellSize));
    }

    // ��ָ�������Ϸ�������
    public void PlaceObjectAtCell(GameObject obj, int x, int y)
    {
        obj.transform.position = GetCellWorldPosition(x, y);
    }
}