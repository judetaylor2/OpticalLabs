using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjector : MonoBehaviour
{
    MeshRenderer meshRenderer;
    ParticleSystem laserParticle;
    ParticleSystem.MainModule laserMainModule;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        laserParticle = transform.GetChild(0).GetComponent<ParticleSystem>();   
        laserMainModule = laserParticle.main; 
    }

    void Update()
    {
        laserMainModule.startColor = meshRenderer.material.color;
    }
}
