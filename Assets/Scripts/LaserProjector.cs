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
        //every laser projectors particle in the hierachy is the first child of the projector
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
        foreach (ParticleSystem g in laserList)
        {
            //cleanup laserList by removing null particle systems
            if (g == null)
            {
                laserList.Remove(g);
                return;
            }
            
            //set the particle's colour to the same as the specified mesh
            ParticleSystem.MainModule laserMainModule = g.main; 
            laserMainModule.startColor = meshRenderer.material.color;

            RaycastHit hit;
            if (Physics.Raycast(g.transform.position, g.transform.forward, out hit, 999, ground | conductiveGround | conductiveMovableGround | conductiveEffectGround | conductive | movableGround))
            {
                Debug.DrawLine(g.transform.position, hit.point, Color.green);
                
                
                GameObject colliderObject = hit.collider.gameObject;

                //set the position, rotation and colour of the filters laser to look seamless
                if(colliderObject.tag == "Filter")
                {
                    if (prevFilter == null) //instantiate a new filter laser for each projector laser
                    prevFilter = Instantiate(colliderObject.transform.GetChild(2));

                    prevFilter.gameObject.SetActive(true); //bring the laser forward slightly to avoid the ray clipping with the laser
                    prevFilter.transform.position = hit.point - transform.GetChild(0).up * 2f;
                    prevFilter.transform.GetChild(0).rotation = g.transform.rotation;

                    //the alpha value is rounded to 2 decimal places since the original value since MinMaxGradient to Color give unnescesary places
                    Color c = g.main.startColor.color;
                    c.a = Mathf.Round(c.a * 100) / 100;
                    
                    //only let the correct colours through
                    if (colliderObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color == (g.main.startColor.color) || c == new Color(1, 1, 1, 0.25f))
                        //change mesh colour since the laser particle gets its start colour from the mesh
                        prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = colliderObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color;
                    else
                        prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
                
                }
                else if (prevFilter != null)
                {
                    //reset colour of the filter laser if not colliding with filter
                    prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
                    prevFilter.gameObject.SetActive(false);
                }
                

                if (colliderObject.transform.tag == "Mirror")
                {
                    colliderObject.transform.GetComponent<Mirror>().isColliding = true;
                    
                    //change colour that mirror reflects
                    MeshRenderer m = colliderObject.transform.GetChild(0).GetComponent<MeshRenderer>();
                    ParticleSystem.MainModule p = colliderObject.transform.GetComponent<Mirror>().laserObject.transform.GetChild(0).GetComponent<ParticleSystem>().main;
                    p.startColor = m.material.color = g.main.startColor.color;

                    //follow the mirror
                    g.transform.LookAt(hit.transform.GetChild(0).position);
                }
                
                
                if (colliderObject.transform.tag == "Sensor")
                {
                    sensorCollider = colliderObject.transform.GetComponent<Sensor>();
                    
                    //check if the R G & B values are the same but not the A
                    if (colliderObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color.r == g.main.startColor.color.r &&
                    colliderObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color.g == g.main.startColor.color.g &&
                    colliderObject.transform.GetChild(1).GetComponent<MeshRenderer>().material.color.b == g.main.startColor.color.b )
                    sensorCollider.isOn = !sendOffSignal;
                    
                    //follow the sensor
                    g.transform.LookAt(hit.transform.position);
                
                }
                
                //deletes laser when there is no target
                if (hit.collider.transform.tag != "Sensor" && hit.collider.transform.tag != "Mirror" && hit.collider.transform.tag != "Filter" && g != laserParticle && !photonGun.isHoldingLaser)
                {
                    Destroy(g.gameObject);
                }
                
            }
            else 
            {
                if (prevFilter != null)
                {
                    prevFilter.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.clear;
                    prevFilter.gameObject.SetActive(false);
                }
                
                if (sensorCollider != null)
                {
                    sensorCollider.isOn = false;
                }

            }
            
            
            //keeps mirror upright
            if (isMirror)
            {
                transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }
}
