using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonGun : MonoBehaviour
{
    public Color[] colours;
    public Transform raycastStartPoint;
    public float photonDistance;
    int currentColourIndex;
    public Image colorImage;
    public LayerMask groundMask;
    public ParticleSystem shootParticle, shootParticleFlash;
    public Animator anim;
    public GameObject laserColourHolder;
    public AudioSource shootSound;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        
        if (Input.GetButtonDown("Fire1"))
        {
            
            RaycastHit hit;
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance, Physics.AllLayers))
            {
                
                Debug.Log ("name = " + LayerMask.LayerToName(hit.collider.gameObject.layer));
                if ((hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 13 || hit.collider.transform.tag == "Filter") && hit.collider.transform.name != "LaserProjector")
                {
                    shootSound.time = Random.Range(0, 2) == 1? 0 : 3;
                    shootSound.Play();
                    
                    anim.SetBool("isShooting", true);
                    
                    p1.startColor = p2.startColor = colours[currentColourIndex];
                    if (!shootParticle.isPlaying) shootParticle.Play();

                    Material m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                    m.color = colours[currentColourIndex];
                    
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
                else if (hit.collider.transform.name == "LaserProjector")
                {
                    Material m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                    m.color = colours[currentColourIndex];
                    hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material = m;
                }
                
            }
            
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            
            RaycastHit hit;
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance))
            if (hit.collider.gameObject.layer == 8 || hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 10 || hit.collider.transform.tag == "Filter")
            {
                shootSound.time = 4.8f;
                shootSound.Play();

                anim.SetBool("isShooting", true);
                
                p1.startColor = p2.startColor = Color.white;
                if (!shootParticle.isPlaying) shootParticle.Play();
                
                Material m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material;
                m.color = Color.white;
                hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material = m;

                hit.collider.transform.tag = "Untagged";
            }
        }
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
}
