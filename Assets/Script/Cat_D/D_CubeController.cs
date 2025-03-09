using UnityEngine;

public class D_CubeController : MonoBehaviour
{
    private float scaleOfCube;

    private Rigidbody rb;
    private bool isFlipping;
    private Vector3 flipAxis;
    private Vector3 pivotPoint;
    private float currentAngle;
    public float RotateSpeed = 90f;

    private void Start()
    {
        scaleOfCube = transform.localScale.x;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (isFlipping)
        {
            ExecuteFlip();
        }
    }

    private void ExecuteFlip()
    {
        if (isFlipping)
        {
            float angleToRotate = RotateSpeed * Time.deltaTime;
            currentAngle += angleToRotate;

            transform.RotateAround(pivotPoint, flipAxis, angleToRotate);

            if (currentAngle >= 90)
            {
                isFlipping = false;
                currentAngle = 0;

                // �����������λ������Ӧ����
                Vector3 position = transform.position;
                position.x = Mathf.Round(position.x * 10) / 10;
                position.y = Mathf.Round(position.y * 10) / 10;
                position.z = Mathf.Round(position.z * 10) / 10;
                transform.position = position;

                // ȷ�����������ת�Ƕ��� 90 �ȵı���
                Vector3 eulerAngles = transform.eulerAngles;
                eulerAngles.x = Mathf.Round(eulerAngles.x / 90) * 90;
                eulerAngles.y = Mathf.Round(eulerAngles.y / 90) * 90;
                eulerAngles.z = Mathf.Round(eulerAngles.z / 90) * 90;
                transform.eulerAngles = eulerAngles;
            }
        }
    }

    private void HandleInput()
    {
        if (isFlipping) return;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input.magnitude < 0.1f) return;

        flipAxis = -Vector3.Cross(input, Vector3.up);
        pivotPoint = transform.position + 0.5f * scaleOfCube * input.normalized;
        pivotPoint.y -= 0.5f * scaleOfCube;

        isFlipping = true;
    }
}
