using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjector : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    ParticleSystem laserParticle;
    public LayerMask ground, movableGround, conductiveGround, conductiveMovableGround, conductiveEffectGround, conductive;
    public PhotonGun photonGun;
    public bool isMirror, sendOffSignal;
    Sensor sensorCollider;
    
    void Start()
    {
        laserParticle = transform.GetChild(0).GetComponent<ParticleSystem>();

        photonGun = GameObject.FindWithTag("Player").GetComponent<PhotonGun>();
    }

    void Update()
    {
        ParticleSystem.MainModule laserMainModule = laserParticle.main; 
        laserMainModule.startColor = meshRenderer.material.color;
        
        RaycastHit hit;
        if (Physics.Raycast(laserParticle.transform.position, laserParticle.transform.forward, out hit, 100, ground | conductiveGround | conductiveMovableGround | conductiveEffectGround | conductive))
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
                else if (meshRenderer.material.color == Color.white)
                {
                    if (hit.collider.transform.gameObject.layer == 10)
                    t.tag = "Untagged";
                    
                    hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                }

            }

            
            Filter filter = laserParticle.GetComponent<Filter>();
            
            if (hit.collider.transform.tag == "Mirror")
            {
                //if (filter != null)
                if (filter.particleList.Count > 0)
                {
                    if (filter.particleList[0].startColor.a > 64)
                    {
                        hit.collider.transform.GetComponentInParent<Mirror>().isColliding = true;
                        
                        MeshRenderer m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>();
                        ParticleSystem.MainModule p = hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                        p.startColor = m.material.color = laserParticle.main.startColor.color;
                        
                    }

                }
                else
                {
                    hit.collider.transform.GetComponentInParent<Mirror>().isColliding = true;
                        
                    MeshRenderer m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>();
                    ParticleSystem.MainModule p = hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                    p.startColor = m.material.color = laserParticle.main.startColor.color;
                    
                    //hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).position = hit.point;
                    //hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.rotation = hit.collider.transform.rotation;
               
                }
                
            }
            
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
                sensorCollider.isOn = true;
                //sensorCollider = null;
            }

        }
    }
}
