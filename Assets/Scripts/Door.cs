using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Sensor sensor;
    Animator anim;
    bool prevSensorValue, animationPlayed;
    public AudioSource doorSound;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sensor.isOn)
        {
            anim.SetBool("DoorOpened", true);
            //animationPlayed = true;

        }
        else if (!sensor.isOn)
        {
            anim.SetBool("DoorOpened", false);
            //animationPlayed = true;
        }

        //play open and close sound before it is in it's idle animation
        if (sensor.isOn && !anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Open_Idle") && !doorSound.isPlaying)
        {
            doorSound.time = 3;
            doorSound.Play();
        }
        else if (!sensor.isOn && !anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Closed_Idle") && !doorSound.isPlaying)
        {
            doorSound.time = 4;
            doorSound.Play();
        }
        
    }
}
