using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjector : MonoBehaviour
{
    MeshRenderer meshRenderer;
    ParticleSystem laserParticle;
    public LayerMask groundMask;
    public PhotonGun photonGun;
    public bool isMirror;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        laserParticle = transform.GetChild(0).GetComponent<ParticleSystem>();

        photonGun = GameObject.FindWithTag("Player").GetComponent<PhotonGun>();
    }

    void Update()
    {
        ParticleSystem.MainModule laserMainModule = laserParticle.main; 
        laserMainModule.startColor = meshRenderer.material.color;
        
        RaycastHit hit;
        if (Physics.Raycast(laserParticle.transform.position, laserParticle.transform.forward, out hit, 100))
        {
                Debug.DrawLine(laserParticle.transform.position, hit.point, Color.green);
                
                Transform t = hit.transform;
                
                if (hit.transform.tag != "Filter")
                for (int i = 0; i < photonGun.colours.Length; i++)
                {
                    if (meshRenderer.material.color == photonGun.colours[i])
                    {
                        if (hit.transform.gameObject.layer == 10)
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
                        if (hit.transform.gameObject.layer == 10)
                        t.tag = "Untagged";
                        
                        hit.transform.GetComponent<MeshRenderer>().material.color = Color.white;
                        break;
                    }

                }

            
            Filter filter = laserParticle.GetComponent<Filter>();
            
            if (filter != null && hit.transform.tag == "Mirror")
            if (filter.particleList.Count > 0)
            {
                if (filter.particleList[0].startColor.a > 64)
                {
                    hit.transform.GetComponentInParent<Mirror>().isColliding = true;
                    
                
                    MeshRenderer m = hit.transform.GetComponent<Mirror>().laserObject.GetComponentInParent<MeshRenderer>();
                    Color32 c = m.material.color;
                    c = filter.particleList[0].startColor;
                    m.material.color = c;
                  
                }

            }
            else if (hit.transform.tag == "Mirror")
            {
                hit.transform.GetComponentInParent<Mirror>().isColliding = true;
                    
                MeshRenderer m = hit.transform.GetComponent<Mirror>().laserObject.GetComponentInParent<MeshRenderer>();
                m.material.color = laserParticle.main.startColor.color;
               
            }
        }
    }
}
