using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour
{
    bool isEnter;
    GameObject triggerCollider;
    List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();
    ParticleSystem particle;
    
    void OnParticleTrigger()
    {
        particle = GetComponent<ParticleSystem>();
        
        int enterNum = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
        Debug.Log("Particle is colliding " + enterNum);

        for (int i = 0; i < enterNum; i++)
        {
            ParticleSystem.Particle p = particleList[i];
            p.startColor = Color.magenta;
            particleList[i] = p;
        }
    }

   
}
