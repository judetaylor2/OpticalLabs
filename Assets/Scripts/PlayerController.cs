using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    
    public float moveSpeed, jumpHeight, groundDrag, gravity, groundDistance, objectDistance;
    Vector3 velocity;
    public Transform groundCheck, objectPickupPoint, cameraPoint;
    public LayerMask groundMask;
    bool isGrounded;
    GameObject currentlyHeldObject;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
        Gizmos.DrawLine(cameraPoint.position, cameraPoint.position + cameraPoint.forward * objectDistance);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        PickupObject();
    }

    void MovePlayer()
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

    void PickupObject()
    {
        if (Input.GetButtonDown("Interact"))
        {
            RaycastHit objectHit;
            Rigidbody r;

            if (currentlyHeldObject == null && Physics.Raycast(cameraPoint.position, cameraPoint.forward, out objectHit, objectDistance))
            {
                if (objectHit.transform.tag != "Player" && objectHit.transform.localScale.x < 5 && objectHit.transform.localScale.y < 5 && objectHit.transform.localScale.z < 5)
                {
                    currentlyHeldObject = objectHit.transform.gameObject;
                    
                    currentlyHeldObject.transform.position = objectPickupPoint.position;
                    currentlyHeldObject.transform.rotation = objectPickupPoint.rotation;
                    currentlyHeldObject.transform.parent = objectPickupPoint;

                    if (currentlyHeldObject.TryGetComponent<Rigidbody>(out r))
                    {
                        r.useGravity = false;
                        
                    }
                    
                }
                
            }
            else if (currentlyHeldObject != null)
            {
                currentlyHeldObject.transform.parent = null;

                if (currentlyHeldObject.TryGetComponent<Rigidbody>(out r))
                {
                    r.useGravity = true;
                }
                
                currentlyHeldObject = null;
            }
        }
    }
}
