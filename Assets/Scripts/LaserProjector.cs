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
            if (hit.transform.childCount > 0)
            {
                Debug.DrawLine(laserParticle.transform.position, hit.point);
                
                Transform t = hit.transform.GetChild(0);
                
                for (int i = 0; i < photonGun.colours.Length; i++)
                {
                    if (hit.transform.tag == "Conductive" && meshRenderer.material.color == photonGun.colours[i])
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

            }
        }
    }
}
