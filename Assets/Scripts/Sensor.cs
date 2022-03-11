using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool isOn, isCollidingWithLaser;
    bool isColliding;
    Collider triggerCollider;
    
    void OnTriggerStay(Collider other)
    {
        triggerCollider = other;
        isColliding = true;
    }

    void Update()
    {
        if (triggerCollider != null && isColliding)
        {
            isOn = triggerCollider.tag == "Conductive" && triggerCollider.GetComponent<MeshRenderer>().material.color == transform.parent.GetComponent<MeshRenderer>().material.color;
            isColliding = false;
        }
        else if (isCollidingWithLaser)
        {
            isOn = true;
        }
        else
        isOn = false;
    }
}
