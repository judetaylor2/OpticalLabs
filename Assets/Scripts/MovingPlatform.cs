using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Rigidbody rb;
    public float moveSpeed;
    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = transform.forward;    
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(moveSpeed * moveDirection * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        moveDirection = -moveDirection;
    }
}
