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

    void OnDrawGizmos() {Debug.DrawLine(transform.position, transform.position + (transform.forward * 5));}
    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * moveDirection * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player") 
        other.transform.parent = transform;
        else 
        moveDirection = -moveDirection;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        other.transform.parent = null;
    }
}
