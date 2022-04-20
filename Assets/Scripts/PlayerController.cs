using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    
    public float moveSpeed, jumpHeight, bounceJumpHeight, groundDrag, gravity, groundDistance, objectDistance, walkSpeed, sprintSpeed;
    Vector3 velocity;
    public Transform groundCheck, objectPickupPoint, cameraPoint;
    public LayerMask ground, movableGround, conductiveGround, conductiveMovableGround, conductiveEffectGround, movable;
    bool isGrounded, isUsingGravityEffect, isTakingDamage;
    GameObject currentlyHeldObject;
    RaycastHit objectHit;
    public ParticleSystem[] pickupParticles;
    public Animator anim;

    float healthStopWatch, healthRegenStopWatch, currentHealth = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);
        Gizmos.DrawLine(cameraPoint.position, cameraPoint.position + cameraPoint.forward * objectDistance);
        Gizmos.DrawSphere(objectHit.point, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        PickupObject();
        Health();
    }

    void MovePlayer()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground | movableGround | conductiveGround | conductiveMovableGround);

        
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
            
            RaycastHit slopeHit;
            Physics.Raycast(transform.position, -transform.up, out slopeHit);
            
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            
            Vector3 move = transform.right * x + transform.forward * z;
            Vector3 slopeDirection = Vector3.ProjectOnPlane(move.normalized, slopeHit.normal);
            rb.AddForce(slopeDirection * (moveSpeed / 10) * Time.deltaTime);
        }





        velocity.y += gravity * Time.deltaTime * 2;
        rb.AddForce(velocity.y * transform.up);
        
    }

    void PickupObject()
    {
        if (Input.GetButtonDown("Interact"))
        {

            Rigidbody r;

            if (currentlyHeldObject == null && Physics.Raycast(cameraPoint.position, cameraPoint.forward, out objectHit, objectDistance, movableGround | conductiveMovableGround | movable))
            {
                anim.SetBool("isPickingUpObject", true);
                
                if (objectHit.transform.tag != "Player" && objectHit.transform.localScale.x < 5 && objectHit.transform.localScale.y < 5 && objectHit.transform.localScale.z < 5)
                {
                    currentlyHeldObject = objectHit.transform.gameObject;
                    
                    currentlyHeldObject.transform.position = objectPickupPoint.position;
                    //currentlyHeldObject.transform.rotation = objectPickupPoint.rotation;
                    currentlyHeldObject.transform.parent = objectPickupPoint;

                    if (currentlyHeldObject.TryGetComponent<Rigidbody>(out r))
                    {
                        r.useGravity = false;
                        r.constraints = RigidbodyConstraints.FreezeRotation;
                    }

                    cameraPoint.GetComponent<CameraController>().isHoldingObject = true;
                    
                }
                
            }
            else if (currentlyHeldObject != null)
            {
                anim.SetBool("isPickingUpObject", false);

                currentlyHeldObject.transform.parent = null;

                if (currentlyHeldObject.TryGetComponent<Rigidbody>(out r))
                {
                    r.constraints = RigidbodyConstraints.None;
                    r.useGravity = true;
                    r.velocity = rb.velocity;
                }
                
                currentlyHeldObject = null;

                cameraPoint.GetComponent<CameraController>().isHoldingObject = false;
                
            }
                
            currentlyHeldObject.transform.rotation = transform.rotation;

        }

            if (currentlyHeldObject != null)
            {
                if (Physics.Raycast(cameraPoint.position, cameraPoint.forward, out objectHit, objectDistance, ground | conductiveGround | conductiveEffectGround))
                objectPickupPoint.position = objectHit.point - cameraPoint.forward * currentlyHeldObject.transform.localScale.y;
                else
                objectPickupPoint.position = cameraPoint.position + cameraPoint.forward * objectDistance;
                
                currentlyHeldObject.transform.position = objectPickupPoint.position;
                for (int i = 0; i < pickupParticles.Length; i++)
                if (!pickupParticles[i].isPlaying)
                pickupParticles[i].Play();

                if (Input.GetButtonDown("Rotate"))
                currentlyHeldObject.transform.Rotate(0,90, 0);
            }
            else
            {
                objectPickupPoint.position = cameraPoint.position + cameraPoint.forward * objectDistance;

                for (int i = 0; i < pickupParticles.Length; i++)
                if (pickupParticles[i].isPlaying)
                pickupParticles[i].Stop();
            }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Speed")
        {
            moveSpeed = sprintSpeed;

            StartCoroutine("LerpToGravity");
            isUsingGravityEffect = false;
        }
        else if (other.tag == "Gravity")
        {
            isUsingGravityEffect = true;

            RaycastHit r;
            if(Physics.Raycast(cameraPoint.position, -(transform.position - other.transform.position), out r, 500, ground | movableGround | conductiveGround | conductiveMovableGround | conductiveEffectGround))
            {
                Vector3 v = Vector3.Cross(transform.right, r.normal);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v, r.normal), 10 * Time.deltaTime);                
            }
            
            moveSpeed = walkSpeed;
        }
        else if (other.tag == "Bounce")
        {
            RaycastHit r;
            if (Physics.Raycast(cameraPoint.position, -(transform.position - other.transform.position), out r, 500, ground | movableGround | conductiveGround | conductiveMovableGround | conductiveEffectGround))
            rb.AddForce(rb.velocity + bounceJumpHeight / 4 * -gravity * r.normal);

            moveSpeed = walkSpeed; 

            StartCoroutine("LerpToGravity");
            isUsingGravityEffect = false;
        }
        else if (other.tag == "Untagged")
        {
            moveSpeed = walkSpeed;

            StartCoroutine("LerpToGravity");
            isUsingGravityEffect = false;
        }
        else if (other.tag == "Electrified Panel")
        {
            isTakingDamage = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Speed")
        {
            moveSpeed = walkSpeed;
        }
        else if (other.tag == "Gravity")
        {
            //transform.up = Vector3.up;
            StartCoroutine("LerpToGravity");
            isUsingGravityEffect = false;
        }
    }

    IEnumerator LerpToGravity()
    {
        yield return new WaitForSeconds(0.1f);

        if (isUsingGravityEffect)
            yield break;

        for (int i = 0; i < 10; i++)
        {
            RaycastHit r;
            if(Physics.Raycast(cameraPoint.position, -Vector3.up, out r, 500, ground | movableGround | conductiveGround | conductiveMovableGround | conductiveEffectGround))
            {
                Vector3 v = Vector3.Cross(transform.right, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(v, Vector3.up), 30 * Time.deltaTime);            
            }

            if (isUsingGravityEffect)
            yield break;
            
            yield return new WaitForSeconds(0.01f);
            
        }
        
    }
    
    void Health()
    {
        healthStopWatch += Time.deltaTime;
        healthRegenStopWatch += Time.deltaTime;

        if (isTakingDamage && healthStopWatch >= 0.5f)
        {
            currentHealth -= 25;
            
            isTakingDamage = false;
            healthStopWatch = 0f;
        }
        else if (healthRegenStopWatch >= 0.25f)
        {
            currentHealth += 5;
            healthRegenStopWatch = 0f;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, 100);

        if (currentHealth == 0)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

