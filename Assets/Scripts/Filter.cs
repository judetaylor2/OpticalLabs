using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.TriggerModule t;
    
    [HideInInspector] public Color32 endColour;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        t = ps.trigger;

        GameObject[] filterObjects = GameObject.FindGameObjectsWithTag("Filter");

        foreach (GameObject g in filterObjects)
        t.AddCollider(g.GetComponent<Component>());

        //CalculateEndColour();
        endColour = new Color();
    }

    void Update()
    {
       // CalculateEndColour();
    }

    void CalculateEndColour()
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int particleCount = ps.GetParticles(particles, ps.particleCount);

        if (particleCount > 0)
        endColour = particles[ps.particleCount - 1].startColor;
        else
        endColour = Color.white;

        Debug.Log(endColour + "is the color");
    }

    void OnParticleTrigger()
    {
        if (ps == null)
        return;
        
        List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();
        int enterNum = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);


        for (int i = 0; i < enterNum; i++)
        {
            ParticleSystem.Particle p = enterParticles[i];
            
            //since there is no way of finding out the object the particle collided with, it is calculated by finding the closest position
            Transform minDistanceTransform = t.GetCollider(0).transform;
            for (int x = 0; x < t.colliderCount; x++)
            {
                if (Vector3.Distance(p.position, t.GetCollider(x).transform.position) < Vector3.Distance(p.position, minDistanceTransform.position))
                minDistanceTransform = t.GetCollider(x).transform;
            }
            
            //the 1st child is where the mesh renderer of the filter is stored
            
            Color32 meshColour = minDistanceTransform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color;
            
            Debug.Log(ps.main.startColor.color.r*255 + " "+ meshColour.r);
            //check if the laser can pass through the filter
            if (ps.main.startColor.color == meshColour || ps.main.startColor.color == new Color(1, 1, 1, 0.25f))
            {
                p.startColor = meshColour;
            }
            else
            {
                p.startColor = Color.clear;
            }

            enterParticles[i] = p;
        }
        
        //set the endColour once the particles have been changed
        if (enterNum > 0)
        endColour = enterParticles[enterNum - 1].startColor;
        else
        endColour = ps.main.startColor.color;

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);
    }
}