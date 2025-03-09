using UnityEngine;

public class CubeController : MonoBehaviour
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
            }
        }
    }

    private void HandleInput()
    {
        if (isFlipping) return;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input.magnitude < 0.1f) return;

        flipAxis = - Vector3.Cross(input, Vector3.up);
        pivotPoint = transform.position + 0.5f * scaleOfCube * input.normalized;
        pivotPoint.y -= 0.5f * scaleOfCube;

        isFlipping = true;
    }
}
