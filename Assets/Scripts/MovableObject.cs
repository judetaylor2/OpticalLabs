using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public AudioSource moveSound;
    
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7 && GetComponent<Rigidbody>().velocity.magnitude != 0 && !moveSound.isPlaying)
        {
            moveSound.time = Random.Range(0, 1) == 1? 0.5f : 2.6f;
            moveSound.Play();
        }
    }

    void Update()
    {
        if ((moveSound.time > 1.4f && moveSound.time < 2.6f) || moveSound.time > 3.5f)
        moveSound.Stop();
    }
}
