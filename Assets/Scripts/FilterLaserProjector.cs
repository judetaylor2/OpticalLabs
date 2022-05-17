using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterLaserProjector : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    ParticleSystem laserParticle;
    public LayerMask ground, movableGround, conductiveGround, conductiveMovableGround, conductiveEffectGround, conductive;
    public PhotonGun photonGun;
    public bool isMirror, sendOffSignal, isFilterLaser;
    Sensor sensorCollider;
    Transform prevFilter;
    
    void Start()
    {
        laserParticle = transform.GetChild(0).GetComponent<ParticleSystem>();

        photonGun = GameObject.FindWithTag("Player").GetComponent<PhotonGun>();
    }

    void OnDrawGizmos()
    {
        if (laserParticle != null)
        Gizmos.DrawSphere(laserParticle.transform.position, 0.1f);
    }

    void Update()
    {
        ParticleSystem.MainModule laserMainModule = laserParticle.main; 
        laserMainModule.startColor = meshRenderer.material.color;
        
        RaycastHit hit;
        if (Physics.Raycast(laserParticle.transform.position, laserParticle.transform.forward, out hit, 999, ground | conductiveGround | conductiveMovableGround | conductiveEffectGround | conductive | movableGround))
        {
            Debug.DrawLine(laserParticle.transform.position, hit.point, Color.green);
            
            Transform t = hit.collider.transform;
            
            if (hit.collider.transform.tag != "Filter" && hit.collider.transform.tag != "Sensor")
            for (int i = 0; i < photonGun.colours.Length; i++)
            {
                if (meshRenderer.material.color == photonGun.colours[i])
                {
                    if (hit.collider.transform.gameObject.layer == 10)
                    {
                        if (i == 0)
                        {
                            t.tag = "Speed";
                        }
                        else if (i == 1)
                        {
                            t.tag = "Gravity";
                        }
                        else if (i == 2)
                        {
                            t.tag = "Bounce";
                        }

                        hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = photonGun.colours[i];                    
                        
                    }
                }
                else if (meshRenderer.material.color == new Color(1, 1, 1, 0.25f))
                {
                    if (hit.collider.transform.gameObject.layer == 10)
                    t.tag = "Untagged";
                    
                    //hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                }

            }

            //set the position, rotation and colour of the filters laser to look seamless
            if(hit.collider.gameObject.tag == "Filter" && !isFilterLaser)
            {
                if (prevFilter == null)
                prevFilter = Instantiate(hit.collider.transform.GetChild(2), hit.collider.transform);
                
                prevFilter.gameObject.SetActive(true);
                
                //transform.GetChild(0).forward is the laser direction
                prevFilter.transform.position = hit.point + transform.GetChild(0).forward * 1.5f;
                prevFilter.transform.rotation = transform.rotation;              
                //filter.laserProjector.GetComponent<LaserProjector>().laserParticle.main.startColor = laserParticle.main.startColor;}

                //the alpha value is rounded to 2 decimal places since the original value since MinMaxGradient to Color give unnescesary places
                Color c = laserParticle.main.startColor.color;
                c.a = Mathf.Round(c.a * 100) / 100;

                if (hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color == (laserParticle.main.startColor.color) || c == new Color(1, 1, 1, 0.25f))
                //get colour from mesh since the laser particle gets its start colour from the mesh
                prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color;
                else
                prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
            }
            //reset colour if not colliding with filter
            else if (prevFilter != null && !isFilterLaser)
            {
                prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
                prevFilter.gameObject.SetActive(false);
            }
            
            if (hit.collider.transform.tag == "Mirror")
            {
                //if (filter != null)
                //if (filter.particleList.Count > 0)
                {
                    //if (filter.particleList[0].startColor.a > 64)
                    {
                        hit.collider.transform.GetComponentInParent<Mirror>().isColliding = true;
                        
                        MeshRenderer m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>();
                        ParticleSystem.MainModule p = hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                        p.startColor = m.material.color = laserParticle.main.startColor.color;
                        
                        //follow the mirror
                        transform.GetChild(0).LookAt(hit.collider.transform.GetChild(0).transform.position);
                        
                    }

                }/*
                else
                {
                    hit.collider.transform.GetComponentInParent<Mirror>().isColliding = true;
                        
                    MeshRenderer m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>();
                    ParticleSystem.MainModule p = hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                    p.startColor = m.material.color = laserParticle.main.startColor.color;
                    
                    //hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).position = hit.point;
                    //hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.rotation = hit.collider.transform.rotation;
               
                }*/
                
            }
            
        }
        //reset colour if not colliding with filter
        else if (prevFilter != null)
        {
            prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
            prevFilter.gameObject.SetActive(false);
        }
        
        RaycastHit hit2;
        if (Physics.Raycast(laserParticle.transform.position, laserParticle.transform.forward, out hit2))
        {
            if (hit2.transform.gameObject.layer == 6)
            {
                if (hit2.collider.transform.tag == "Sensor")
                {
                    sensorCollider = hit2.collider.transform.GetComponent<Sensor>();
                    
                    if (hit2.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color == laserParticle.main.startColor.color)
                    sensorCollider.isOn = !sendOffSignal;
                
                }
                
            }
            else if (sensorCollider != null)
            {
                sensorCollider.isOn = false;
                //sensorCollider = null;
            }

        }
    }
}