using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : MonoBehaviour
{
    [HideInInspector] public List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();
    ParticleSystem particle;
    [HideInInspector] public GameObject[] possibleTriggers;
    
    void OnParticleTrigger()
    {
        particle = GetComponent<ParticleSystem>();
        
        int enterNum = particle.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
        Debug.Log("Particle is colliding " + enterNum);

        possibleTriggers = GameObject.FindGameObjectsWithTag("Inverter");
        GameObject correctTrigger;
        
        foreach (GameObject g in possibleTriggers)
        particle.trigger.RemoveCollider(g.GetComponent<Collider>());
        foreach (GameObject g in possibleTriggers)
        particle.trigger.AddCollider(g.GetComponent<Collider>());

        if (possibleTriggers.Length > 0)
        {
            correctTrigger = possibleTriggers[0];
            
            Color32 c;
        
            for (int i = 0; i < enterNum; i++)
            {
                ParticleSystem.Particle p = particleList[i];
                
                foreach (GameObject g in possibleTriggers)
                correctTrigger = Vector3.Distance(g.transform.position, p.position) < Vector3.Distance(correctTrigger.transform.position, p.position)? g : correctTrigger;

    
                Color32 colour = new Color(255 - p.startColor.r, 255 - p.startColor.g, 255 - p.startColor.b, 255);

                p.startColor = colour;
                
                particleList[i] = p;    
            }
            
        }

            
        particle.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
        
    }

   
}
