using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Sensor sensor;
    Animator anim;
    bool prevSensorValue, animationPlayed;
    
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
        
    }
}
