using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [HideInInspector] public bool isColliding, wasColliding;
    public GameObject laserObject;
    public ParticleSystem collidingParticle;
    public AudioSource collidingSound;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //only activate the laser project when laser is colliding. isColliding is set in the LaserProjector script
        if (isColliding)
        {
            //laserObject.SetActive(true);

            if (!collidingParticle.isPlaying) collidingParticle.Play();
        }
        else
        {
            //remove lasers and reset the laser list when not colliding
            if (laserObject.GetComponent<LaserProjector>().laserList.Count > 0)
            {
                
                for (int i = 0; i < laserObject.GetComponent<LaserProjector>().laserList.Count; i++)
                laserObject.GetComponent<LaserProjector>().DeleteLaser(laserObject.GetComponent<LaserProjector>().laserList[i]);
            }
            
            if (collidingParticle.isPlaying) collidingParticle.Stop();

        }
        wasColliding = isColliding;
        
        isColliding = false;

        ParticleSystem.MainModule m = collidingParticle.main;
        m.startColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;

        if (wasColliding && !collidingSound.isPlaying)
        collidingSound.Play();
        else if (!wasColliding)
        collidingSound.Stop();
    }
}
