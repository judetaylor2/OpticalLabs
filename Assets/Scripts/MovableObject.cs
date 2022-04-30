using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public AudioSource moveSound;
    
    void OnCollisionStay(Collision other)
    {
        //play move sound when moving fast enough across the ground
        if (other.gameObject.layer == 6 && GetComponent<Rigidbody>().velocity.magnitude >= 2 && !moveSound.isPlaying)
        {
            moveSound.time = 2.6f;
            moveSound.Play();
        }
    }

    void Update()
    {
        if (moveSound.time > 3.5f || GetComponent<Rigidbody>().velocity.magnitude < 2)
        moveSound.Stop();
    }
}
