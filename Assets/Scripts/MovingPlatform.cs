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
        rb.AddForce(moveSpeed * moveDirection * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag != "Player")
        moveDirection = -moveDirection;
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player") 
        other.transform.parent = transform;

        other.GetComponent<Rigidbody>().AddForce((moveSpeed * moveDirection * Time.deltaTime) / 2);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        other.transform.parent = null;
    }
}
