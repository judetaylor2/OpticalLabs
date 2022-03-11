using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float moveSpeed;
    
    void OnTriggerStay(Collider other)
    {
        Rigidbody rb;
        if (other.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(moveSpeed * transform.forward * Time.deltaTime);
        }
    }
}
