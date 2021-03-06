using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    
    public float moveSpeed, jumpHeight, bounceJumpHeight, groundDrag, gravity, groundDistance, objectDistance, walkSpeed, sprintSpeed;
    Vector3 velocity, panelKnockbackDirection;
    public Transform groundCheck, objectPickupPoint, cameraPoint;
    public LayerMask ground, movableGround, conductiveGround, conductiveMovableGround, conductiveEffectGround, movable, conductive;
    bool isGrounded, isUsingGravityEffect, isTakingDamage, isJumping;
    GameObject currentlyHeldObject;
    RaycastHit objectHit;
    public ParticleSystem[] pickupParticles;
    public Animator anim;
    public AudioSource moveSound1, moveSound2, pickupSound, damageSound, fallSound;

    float healthStopWatch, healthRegenStopWatch, currentHealth = 100;
    public Gradient healthGradient;
    public Image damageUI;
    
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

    void FixedUpdate()
    {
            if (isJumping && isGrounded)
            {
                rb.AddForce(jumpHeight * 1.5f * transform.up);
                isJumping = false;
            }

    }

    void MovePlayer()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground | movableGround | conductiveGround | conductiveMovableGround | conductiveEffectGround);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
            
            rb.drag = groundDrag;
            
            //move player relative to ground slope direction
            RaycastHit slopeHit;
            Physics.Raycast(transform.position, -transform.up, out slopeHit);          
            
            Vector3 move = transform.right * x + transform.forward * z;
            Vector3 slopeDirection = Vector3.ProjectOnPlane(move.normalized, slopeHit.normal);
            rb.AddForce(slopeDirection * moveSpeed * Time.deltaTime);

            

            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
            }

            fallSound.Stop();
        }
        else
        {
            if (velocity.y < -50 && !fallSound.isPlaying)
            fallSound.Play();
            
            rb.drag = 0;
            
            RaycastHit slopeHit;
            Physics.Raycast(transform.position, -transform.up, out slopeHit);
            
            Vector3 move = transform.right * x + transform.forward * z;
            Vector3 slopeDirection = Vector3.ProjectOnPlane(move.normalized, slopeHit.normal);
            rb.AddForce(slopeDirection * (moveSpeed / 10) * Time.deltaTime);
        }

        if (moveSpeed == sprintSpeed)
        {
            //play move sound on speed surface
            if (isGrounded && (x != 0 || z != 0) && (!moveSound2.isPlaying || moveSound2.time >= 10f))
            {
                moveSound2.time = 6.3f;
                moveSound2.Play();
            }
        }
        else
        {
            //play move sound on ground
            if (isGrounded && (x != 0 || z != 0) && (!moveSound1.isPlaying || moveSound1.time >= 10f))
            {
                moveSound1.time = 6.3f;
                moveSound1.Play();
            }
            
        }

        
        if (!isGrounded || (x == 0 && z == 0))
        {
            moveSound1.Stop();
            moveSound2.Stop();
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
                pickupSound.pitch = 1;
                pickupSound.Play();
                anim.SetBool("isPickingUpObject", true);
                
                if (objectHit.transform.tag != "Player" && objectHit.transform.localScale.x < 5 && objectHit.transform.localScale.y < 5 && objectHit.transform.localScale.z < 5)
                {
                    currentlyHeldObject = objectHit.transform.gameObject;
                    
                    currentlyHeldObject.transform.position = objectPickupPoint.position;
                    //currentlyHeldObject.transform.rotation = objectPickupPoint.rotation;
                    currentlyHeldObject.transform.parent = objectPickupPoint;

                    if (currentlyHeldObject.TryGetComponent<Rigidbody>(out r))
                    {
                        r.velocity = Vector3.zero;
                        r.useGravity = false;
                        r.constraints = RigidbodyConstraints.FreezeRotation;
                    }

                    cameraPoint.GetComponent<CameraController>().isHoldingObject = true;
                    
                }

                //currentlyHeldObject.transform.rotation = transform.rotation;
                
            }
            else if (currentlyHeldObject != null)
            {
                pickupSound.pitch = 0.75f;
                pickupSound.Play();
                anim.SetBool("isPickingUpObject", false);

                currentlyHeldObject.transform.parent = null;
                SceneManager.MoveGameObjectToScene(currentlyHeldObject, SceneManager.GetActiveScene());

                if (currentlyHeldObject.TryGetComponent<Rigidbody>(out r))
                {
                    r.transform.rotation = Quaternion.identity;
                    r.constraints = RigidbodyConstraints.None;
                    r.constraints = RigidbodyConstraints.FreezeRotation;
                    r.useGravity = true;
                    r.velocity = rb.velocity;
                }
                
                //currentlyHeldObject.transform.rotation = transform.rotation;
                
                currentlyHeldObject = null;

                cameraPoint.GetComponent<CameraController>().isHoldingObject = false;
                
            }
                

        }

            if (currentlyHeldObject != null)
            {
                if (Physics.Raycast(cameraPoint.position, cameraPoint.forward, out objectHit, objectDistance, ground | conductiveGround | conductiveEffectGround | conductive))
                objectPickupPoint.position = objectHit.point - cameraPoint.forward * currentlyHeldObject.transform.localScale.y * 2;
                else
                objectPickupPoint.position = cameraPoint.position + cameraPoint.forward * objectDistance;
                
                currentlyHeldObject.transform.position = objectPickupPoint.position;
                for (int i = 0; i < pickupParticles.Length; i++)
                if (!pickupParticles[i].isPlaying)
                pickupParticles[i].Play();

                //if (Input.GetButtonDown("Rotate"))
                //currentlyHeldObject.transform.Rotate(0,90, 0);
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
        else if (other.tag == "Bounce" && isGrounded)
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
            panelKnockbackDirection = transform.position - other.transform.position;
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
            rb.AddForce(panelKnockbackDirection * 10000 * Time.deltaTime);
            
            damageSound.time = 6.4f;
            damageSound.pitch = Random.Range(1f, 2f);
            damageSound.Play();
            
            currentHealth -= 25;
            
            isTakingDamage = false;
            healthStopWatch = 0f;

            StartCoroutine("DamageUI");
        }
        else if (healthRegenStopWatch >= 0.25f)
        {
            currentHealth += 2.5f;
            healthRegenStopWatch = 0f;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, 100);

        if (currentHealth <= 0)
        {
            transform.position = GameObject.Find("Level Transition Entrance").transform.GetChild(1).position;
            currentHealth = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    //if health decreases, the screen will become more red and will gradualy turn back
    IEnumerator DamageUI()
    {
        for (int i = 0; i < 10; i++)
        {
            damageUI.color = Color.Lerp(damageUI.color, healthGradient.Evaluate(1 / currentHealth), 10 * Time.deltaTime);
            yield return new WaitForSeconds(0.0001f);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 100; i++)
        {
            damageUI.color = Color.Lerp(damageUI.color, Color.clear, 1 * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
    }
}

