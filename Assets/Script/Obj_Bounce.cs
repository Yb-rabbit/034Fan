using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_Bounce : MonoBehaviour
{
    public float initialBounceForce = 10f; // 初始弹跳力
    public int maxBounceCount = 5; // 最大弹跳次数
    private int currentBounceCount = 0; // 当前弹跳次数
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && currentBounceCount < maxBounceCount)
        {
            float bounceForce = initialBounceForce * (1f - (float)currentBounceCount / maxBounceCount);
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            currentBounceCount++;
        }
    }
}
