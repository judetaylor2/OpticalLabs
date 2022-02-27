using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    
    public float moveSpeed, jumpHeight, groundDrag, gravity, groundDistance;
    Vector3 velocity;
    public Transform groundCheck;
    public LayerMask groundMask;
    bool isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        
        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
            
            rb.drag = groundDrag;
            
            RaycastHit slopeHit;
            Physics.Raycast(transform.position, -transform.up, out slopeHit);
            
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            
            Vector3 move = transform.right * x + transform.forward * z;
            Vector3 slopeDirection = Vector3.ProjectOnPlane(move.normalized, slopeHit.normal);
            rb.AddForce(slopeDirection * moveSpeed * Time.deltaTime);

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(jumpHeight * -2f * gravity * transform.up);
            }
        }
        else
        {
            rb.drag = 0;
        }





        velocity.y += gravity * Time.deltaTime * 2;
        rb.AddForce(velocity);
    }
}
