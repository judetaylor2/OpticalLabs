using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float moveSpeed, offset, conveyorBeltMoveSpeed;
    public Renderer meshRenderer;
    
    void OnTriggerStay(Collider other)
    {
        //push any object that has a rigidbody
        Rigidbody rb;
        if (other.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(moveSpeed * transform.forward * Time.deltaTime);
            rb.AddForce(moveSpeed * Vector3.down * Time.deltaTime);
        }
    }

    void Update()
    {
        //offset the conveyor belt texture to make it look like it's moving
        offset -= conveyorBeltMoveSpeed * Time.deltaTime;

        if (offset >= 5) offset = 0;
        
        meshRenderer.material.mainTextureOffset = new Vector2(0, offset);
    }
}
