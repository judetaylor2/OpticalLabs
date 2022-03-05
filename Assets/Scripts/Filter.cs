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
                p.startColor = c;
                particleList[i] = p;
                Debug.Log("Particle222");
                
            }
                
        }

        particle.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
    }

   
}
