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

                c.r += p.startColor.r;
                c.g += p.startColor.g;
                c.b += p.startColor.b;
                c.a += p.startColor.a;
                
                c.r -= 255;
                c.g -= 255;
                c.b -= 255;
                c.a -= 255;
                
                Debug.Log(c.r + " " + c.g + " " + c.b);
                
                p.startColor = c;
                particleList[i] = p;
                
            }
                
        }

        particle.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
    }

   
}
