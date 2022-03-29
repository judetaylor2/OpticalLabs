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
        if (!animationPlayed && sensor.isOn)
        {
            anim.Play("Door_Open");
            animationPlayed = true;

        }
        else if (!animationPlayed && !sensor.isOn)
        {
            anim.Play("Door_Close");
            animationPlayed = true;
        }
        
        if (sensor.isOn != prevSensorValue)
        animationPlayed = false;
        
        prevSensorValue = sensor.isOn;
        
    }
}
