using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : MonoBehaviour
{
    [HideInInspector] public List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();
    ParticleSystem particle;
    [HideInInspector] public GameObject[] possibleTriggers;
    public Color32 colour;
    
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
        
            for (int i = 0; i < enterNum; i++)
            {
                ParticleSystem.Particle p = particleList[i];
                
                foreach (GameObject g in possibleTriggers)
                correctTrigger = Vector3.Distance(g.transform.position, p.position) < Vector3.Distance(correctTrigger.transform.position, p.position)? g : correctTrigger;

                Vector4 c = new Vector4(p.startColor.r, p.startColor.g, p.startColor.b, 255);
                colour = new Color32((byte)(255 - c.x), (byte)(255 - c.y), (byte)(255 - c.z), 255);
                Debug.Log("c.r: " + (byte)(255 - c.x) + " " + c.x);
                Debug.Log("c.g: " + (byte)(255 - c.y) + " " + c.y);
                Debug.Log("c.b: " + (byte)(255 - c.z) + " " + c.z);

                p.startColor = colour;
                
                particleList[i] = p;    
            }
            
        }

            
        particle.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
        
    }

   
}
