using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjector : MonoBehaviour
{
    MeshRenderer meshRenderer;
    ParticleSystem laserParticle;
    public LayerMask groundMask;
    public PhotonGun photonGun;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        laserParticle = transform.GetChild(0).GetComponent<ParticleSystem>();   
    }

    void Update()
    {
        //ParticleSystem.MainModule laserMainModule = laserParticle.main; 
        //laserMainModule.startColor = meshRenderer.material.color;
        
        RaycastHit hit;
        if (Physics.Raycast(laserParticle.transform.position, laserParticle.transform.forward, out hit, 100))
        {
            /*if (hit.transform.childCount > 0)
            {
                Debug.DrawLine(laserParticle.transform.position, hit.point);
                
                Transform t = hit.transform.GetChild(0);
                
                for (int i = 0; i < photonGun.colours.Length; i++)
                {
                    if (meshRenderer.material.color == photonGun.colours[i])
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

                        hit.transform.GetComponent<MeshRenderer>().material.color = photonGun.colours[i];                    
                    }
                    else if (meshRenderer.material.color == Color.white)
                    {
                        t.tag = "Untagged";
                        hit.transform.GetComponent<MeshRenderer>().material.color = Color.white;
                        break;
                    }

                }

            }*/
            
            Filter filter = laserParticle.GetComponent<Filter>();
            
            if (filter != null && hit.transform.tag == "Mirror")
            if (filter.particleList.Count > 0)
            {
                if (filter.particleList[0].startColor.a > 64)
                {
                    hit.transform.GetComponentInParent<Mirror>().isColliding = true;
                    
                
                    ParticleSystem.MainModule m = hit.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                    ParticleSystem.MinMaxGradient c = m.startColor;
                    c.color = filter.particleList[0].startColor;
                    m.startColor = c;
                  
                }

            }
            else if (hit.transform.tag == "Mirror")
            {
                hit.transform.GetComponentInParent<Mirror>().isColliding = true;
                    
                ParticleSystem.MainModule m = hit.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                m.startColor = laserParticle.main.startColor.color;
               
            }
        }
    }
}
