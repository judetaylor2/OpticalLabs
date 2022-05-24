using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonGun : MonoBehaviour
{
    public Color[] colours;
    public Transform raycastStartPoint, pickupPoint, camPoint;
    public float photonDistance;
    int currentColourIndex;
    public Image colorImage;
    public LayerMask groundMask, movableGround, conductiveGround, conductiveMovableGround, conductiveEffectGround, movable, conductive;
    public ParticleSystem shootParticle, shootParticleFlash;
    public Animator anim;
    public GameObject laserColourHolder;
    public AudioSource shootSound, pickupSound;
    public bool isHoldingLaser;
    GameObject currentLaser;
    
    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(colorImage.canvas.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if ((shootSound.time >= 4 && shootSound.time < 4.8f) || (shootSound.time > 0.7 && shootSound.time < 0.9f) || shootSound.time >= 5.5f)
        {
            shootSound.Stop();
        }

        ParticleSystem.MainModule p1 = shootParticle.main;
        ParticleSystem.MainModule p2 = shootParticleFlash.main;
        
        if (SceneManager.GetActiveScene().name != "Level 1")
        {
            
            //shoot colour
            if (Input.GetButtonDown("Fire2"))
            {
                
                RaycastHit hit;
                if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance))
                {
                    //ground layers
                    if ((hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 13 || hit.collider.transform.tag == "Filter") && hit.collider.transform.name != "LaserProjector")
                    {
                        shootSound.time = Random.Range(0, 2) == 1? 0 : 3;
                        shootSound.Play();
                        
                        anim.SetBool("isShooting", true);
                        
                        p1.startColor = p2.startColor = colours[currentColourIndex];
                        shootParticle.Play();

                        Material m;
                        m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material;

                        m.color = colours[currentColourIndex];
                        
                        //change tag if the layer is conductiveEffectGround
                        if (hit.collider.gameObject.layer == 10)
                        if (currentColourIndex == 0)
                        {
                            //speedCollider.transform.localScale = hit.collider.transform.localScale;
                            //Instantiate(speedCollider, hit.collider.transform.position, hit.collider.transform.rotation);

                            hit.collider.transform.tag = "Speed";
                        }
                        else if (currentColourIndex == 1)
                        {
                            hit.collider.transform.tag = "Gravity";
                        }
                        else if (currentColourIndex == 2)
                        {
                            hit.collider.transform.tag = "Bounce";
                        }                    
                        
                    }
                    //change laser projectors colour
                    else if (hit.collider.transform.name.Contains("LaserProjector"))
                    {
                        shootSound.time = Random.Range(0, 2) == 1? 0 : 3;
                        shootSound.Play();
                        
                        anim.SetBool("isShooting", true);

                        p1.startColor = p2.startColor = colours[currentColourIndex];
                        shootParticle.Play();
                        
                        Material m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                        m.color = colours[currentColourIndex];
                        hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material = m;
                    }
                    
                }
                
            }//clear colour
            else if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                
                RaycastHit hit;
                if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance))
                if (hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 13 || (hit.collider.transform.tag == "Filter" && hit.collider.gameObject.layer == 13))
                {
                    shootSound.time = 4.8f;
                    shootSound.Play();

                    anim.SetBool("isShooting", true);
                    
                    p1.startColor = p2.startColor = new Color(1, 1, 1, 0.25f);
                    shootParticle.Play();
                    
                    if (hit.collider.gameObject.layer != 10)
                    {
                        Material m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                        m.color = new Color(1, 1, 1, 0.25f);
                        hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material = m;

                    }
                    else if (hit.collider.gameObject.layer == 10)
                    {
                        hit.collider.transform.tag = "Untagged";

                        Material m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                        m.color = new Color(1, 1, 1, 0.25f);
                        hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material = m;
                    }

                    if (hit.collider.transform.tag != "Filter")
                    hit.collider.transform.tag = "Untagged";

                }
            }//switch colour
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (currentColourIndex == colours.Length - 1)
                currentColourIndex = 0;
                else 
                currentColourIndex++;

                laserColourHolder.transform.Rotate(120, 0, 0);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (currentColourIndex == 0)
                currentColourIndex = colours.Length - 1;
                else 
                currentColourIndex--;

                laserColourHolder.transform.Rotate(-120, 0, 0);
                
            }

            Color c = colours[currentColourIndex];
            c.a = 1;
            colorImage.color = c;
        }


        {
            RaycastHit hit;
            Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance);

            //drag lasers
            if (Input.GetButtonDown("Fire1"))
            {
                if (isHoldingLaser && hit.collider == null)
                {
                    Destroy(currentLaser);
                    anim.SetBool("isPickingUpObject", false);

                    isHoldingLaser = false;
                    
                    return;
                }
                else if (hit.collider == null)
                {
                    return;
                }
            
                if ((hit.collider.transform.name.Contains("LaserProjector") || hit.collider.transform.name.Contains("Mirror")  || hit.collider.transform.name.Contains("Filter")) 
                && !isHoldingLaser && hit.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ParticleSystem>().main.startColor.color != Color.clear)
                {
                    pickupSound.pitch = 1f;
                    pickupSound.Play();
                    
                    if (hit.collider.transform.name.Contains("LaserProjector"))
                    currentLaser = Instantiate(hit.transform.GetChild(0).gameObject, hit.transform.GetChild(0).position, hit.transform.GetChild(0).rotation, hit.transform);
                    else
                    {
                        //prevents player from being able to create invisible lasers
                        if (hit.collider.GetComponent<Mirror>())
                        if (hit.collider.transform.name.Contains("Mirror"))
                        currentLaser = Instantiate(hit.transform.GetChild(1).GetChild(0).gameObject, hit.transform.GetChild(1).GetChild(0).position, hit.transform.GetChild(1).GetChild(0).rotation, hit.transform.GetChild(1));
                        
                    }
                    
                    if (currentLaser != null)
                    {
                        anim.SetBool("isPickingUpObject", true);
                        currentLaser.transform.parent.GetComponent<LaserProjector>().laserList.Add(currentLaser.GetComponent<ParticleSystem>());
                        currentLaser.SetActive(true);
                        isHoldingLaser = true;

                    }
                    
                }
                else if (isHoldingLaser)
                {
                    Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance);
                    if (hit.collider.tag != "Mirror" && hit.collider.tag != "Sensor" && hit.collider.tag != "Filter")
                    Destroy(currentLaser);

                    //prevent mirror from connecting to itself
                    if (hit.collider.tag == "Mirror" && currentLaser.transform.parent.parent != null)
                    if (hit.collider.gameObject == currentLaser.transform.parent.parent.gameObject)
                    Destroy(currentLaser);
                    
                    //else if (hit.collider.tag == "Sensor")
                    //hit.collider.GetComponent<Sensor>().isCollidingWithLaser = true;
                    
                    pickupSound.pitch = 0.75f;
                    pickupSound.Play();

                    anim.SetBool("isPickingUpObject", false);

                    isHoldingLaser = false;
                }
                
                
            }
            
            if (hit.collider != null)
            //if (hit.collider.transform.name.Contains("LaserProjector"))
            {
                ParticleSystem ps1;
                
                if (hit.collider.tag == "LaserProjector")
                for (int i = 0; i < hit.transform.childCount; i++)
                {
                    if (i != 0 && hit.transform.GetChild(i).TryGetComponent<ParticleSystem>(out ps1))
                    {
                        ParticleSystem ps2 = hit.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
                        
                        ParticleSystem.MainModule m1 = ps1.main;
                        m1.startColor = ps2.main.startColor;
                        
                        if (Input.GetKeyDown(KeyCode.R) && i != 0)
                        {
                            //delete last placed laser
                            ps1.GetComponentInParent<LaserProjector>().DeleteLaser(ps1);
                            isHoldingLaser = false;
                            Debug.Log("Laser removed");
                            break;
                        }
                    }
                    //else break;
                    
                }

                if (hit.collider.tag == "Mirror")
                for (int i = 0; i < hit.transform.GetChild(1).childCount; i++)
                {
                    if (i != 0 && hit.transform.GetChild(1).GetChild(i).TryGetComponent<ParticleSystem>(out ps1))
                    {
                        ParticleSystem ps2 = hit.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ParticleSystem>();
                        
                        ParticleSystem.MainModule m1 = ps1.main;
                        m1.startColor = ps2.main.startColor;
                        
                        if (Input.GetKeyDown(KeyCode.R) && i != 0)
                        {
                            //delete last placed laser
                            ps1.GetComponentInParent<LaserProjector>().DeleteLaser(ps1);
                            isHoldingLaser = false;
                            Debug.Log("Laser removed");
                            break;
                        }
                    }
                    //else break;
                    
                }
                
            }

        }

        //is Holding
        if (isHoldingLaser && currentLaser != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance, groundMask | movableGround | conductiveGround | conductiveMovableGround | conductiveEffectGround | movable))
            currentLaser.transform.LookAt(hit.point);
            else
            currentLaser.transform.LookAt(camPoint.transform.position + camPoint.transform.forward * 50);
        }

    }
}
