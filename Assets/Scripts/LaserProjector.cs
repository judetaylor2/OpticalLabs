using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjector : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    ParticleSystem laserParticle;
    public LayerMask ground, movableGround, conductiveGround, conductiveMovableGround, conductiveEffectGround, conductive;
    public PhotonGun photonGun;
    public bool isMirror, sendOffSignal, isFilterLaser;
    Sensor sensorCollider;
    Transform prevFilter;
    public List<ParticleSystem> laserList;
    List<Vector3> laserDirection;
    
    void Start()
    {
        laserParticle = transform.GetChild(0).GetComponent<ParticleSystem>();

        photonGun = GameObject.FindWithTag("Player").GetComponent<PhotonGun>();
        
        laserList.Add(laserParticle);
    }

    void OnDrawGizmos()
    {
        if (laserParticle != null)
        Gizmos.DrawSphere(laserParticle.transform.position, 0.1f);
    }

    void Update()
    {
        //int particleCount = 0;
        foreach (ParticleSystem g in laserList)
        {
            if (g == null)
            {
                laserList.Remove(g);
                return;
            }
            
            //do not check for the first laser particle since it is invisible
            if (g.gameObject.transform != laserParticle.gameObject.transform)
            {
                ParticleSystem.MainModule laserMainModule = g.main; 
                laserMainModule.startColor = meshRenderer.material.color;
            }
            
            /*if (particleCount == 0)
            return;
            else
            {
                particleCount++;

            }*/
            

            RaycastHit hit;
            if (Physics.Raycast(g.transform.position, g.transform.forward, out hit, 999, ground | conductiveGround | conductiveMovableGround | conductiveEffectGround | conductive))
            {
                Debug.DrawLine(g.transform.position, hit.point, Color.green);
                
                Transform t = hit.collider.transform;
                
                if (g != laserParticle)
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
                    prevFilter = Instantiate(hit.collider.transform.GetChild(2));
                    
                    prevFilter.gameObject.SetActive(true);
                    
                    //transform.GetChild(0).forward is the laser direction
                    prevFilter.transform.position = hit.point + transform.GetChild(0).forward * 2f;
                    //prevFilter.transform.GetChild(0).transform.LookAt(transform);
                    prevFilter.transform.GetChild(0).rotation = g.transform.rotation;
                    //prevFilter.GetChild(0).rotation = Quaternion.LookRotation(transform.GetChild(0).position);           
                    //filter.laserProjector.GetComponent<LaserProjector>().laserParticle.main.startColor = laserParticle.main.startColor;}

                    //the alpha value is rounded to 2 decimal places since the original value since MinMaxGradient to Color give unnescesary places
                    Color c = g.main.startColor.color;
                    c.a = Mathf.Round(c.a * 100) / 100;

                    if (hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color == (g.main.startColor.color) || c == new Color(1, 1, 1, 0.25f))
                    {
                        prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color;
                    }
                    //get colour from mesh since the laser particle gets its start colour from the mesh
                    else
                    {
                        prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
                    }
                }
                //reset colour if not colliding with filter
                else if (prevFilter != null && !isFilterLaser)
                {
                    prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
                    prevFilter.gameObject.SetActive(false);
                }
                
                if (g != laserParticle)
                if (hit.collider.transform.tag == "Mirror")
                {
                        {
                            hit.collider.transform.GetComponentInParent<Mirror>().isColliding = true;
                            
                            MeshRenderer m = hit.collider.transform.GetChild(1).GetComponent<MeshRenderer>();
                            ParticleSystem.MainModule p = hit.collider.transform.GetComponentInParent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                            p.startColor = m.material.color = g.main.startColor.color;

                            //follow the mirror
                            g.transform.LookAt(hit.transform.position);
                            
                        }


                    /*
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
            if (Physics.Raycast(g.transform.position, g.transform.forward, out hit2))
            {
                if (hit2.transform.gameObject.layer == 6)
                {
                    if (hit2.collider.transform.tag == "Sensor")
                    {
                        sensorCollider = hit2.collider.transform.GetComponent<Sensor>();
                        
                        /*//prevent sensor being left on when laser is invisible
                        if (isMirror && !GetComponentInParent<Mirror>().isColliding && sensorCollider.isOn)
                        {
                            sensorCollider.isOn = false;    
                        }
                        else*/
                        {
                            if (hit2.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color.r == g.main.startColor.color.r &&
                            hit2.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color.g == g.main.startColor.color.g &&
                            hit2.collider.transform.GetChild(1).GetComponent<MeshRenderer>().material.color.b == g.main.startColor.color.b )
                            sensorCollider.isOn = !sendOffSignal;
                        }
                        

                        //follow the sensor
                        g.transform.LookAt(hit.transform.position);
                    
                    }
                    
                }
                else if (sensorCollider != null)
                {
                    sensorCollider.isOn = false;
                    //sensorCollider = null;
                }

            }
            else if (sensorCollider != null)
            {
                sensorCollider.isOn = false;
                //sensorCollider = null;
            }
            
            //deletes laser when there is no target
            if (hit.collider != null)
            if (hit.collider.transform.tag != "Sensor" && hit.collider.transform.tag != "Mirror" && hit.collider.transform.tag != "Filter" && g != laserParticle && !photonGun.isHoldingLaser)
            {
                Destroy(g.gameObject);
            }
        }
        }
}
