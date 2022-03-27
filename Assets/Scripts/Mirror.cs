using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [HideInInspector] public bool isColliding;
    public GameObject laserObject;
    public ParticleSystem collidingParticle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            laserObject.SetActive(true);

            if (!collidingParticle.isPlaying) collidingParticle.Play();
        }
        else
        {
            laserObject.SetActive(false);
            
            if (collidingParticle.isPlaying) collidingParticle.Stop();
        }
        
        isColliding = false;

        ParticleSystem.MainModule m = collidingParticle.main;
        m.startColor = transform.GetComponentInChildren<ParticleSystem>().main.startColor;
    }
}
