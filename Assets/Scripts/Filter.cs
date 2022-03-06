using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    public GameObject triggerCollider;
    List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();
    ParticleSystem particle;
    
    void OnParticleTrigger()
    {
        particle = GetComponent<ParticleSystem>();
        
        int enterNum = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
        Debug.Log("Particle is colliding " + enterNum);

        GameObject[] possibleTriggers = GameObject.FindGameObjectsWithTag("Filter");

        Color32 c;
        
        for (int i = 0; i < enterNum; i++)
        {
            foreach (GameObject g in possibleTriggers)
            {
                ParticleSystem.Particle p = particleList[i];

                c = g.GetComponent<MeshRenderer>().material.color;

                if (p.startColor.r >= c.r && p.startColor.g >= c.g && p.startColor.b >= c.b && p.startColor.a >= c.a)
                p.startColor = c;
                else
                p.startColor = Color.clear;
                
                particleList[i] = p;
                
            }
                
        }

        particle.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
    }

   
}
