using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Sensor[] sensor;
    Animator anim;
    bool prevSensorValue, animationPlayed, allSensorsActive;
    public AudioSource doorSound;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Sensor s in sensor)
        {
            if (s.isOn)
            {
                allSensorsActive = true;        
            }
            else
            {
                allSensorsActive = false;
                break;
            }

        }

        if (allSensorsActive)
        {
            anim.SetBool("DoorOpened", true);
            //animationPlayed = true;

        }
        else if (!allSensorsActive)
        {
            anim.SetBool("DoorOpened", false);
            //animationPlayed = true;
        }

        //play open and close sound before it is in it's idle animation
        if (allSensorsActive && !anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Open_Idle") && !doorSound.isPlaying)
        {
            doorSound.time = 3;
            doorSound.Play();
        }
        else if (!allSensorsActive && !anim.GetCurrentAnimatorStateInfo(0).IsName("Door_Closed_Idle") && !doorSound.isPlaying)
        {
            doorSound.time = 4;
            doorSound.Play();
        }
        
    }
}
