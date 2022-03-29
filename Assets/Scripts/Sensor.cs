using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool isOn;
    bool isColliding;
    Collider triggerCollider;
    
    void OnTriggerStay(Collider other)
    {
        triggerCollider = other;
        //isColliding = true;
        
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        isOn = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9)
        isOn = false;
    }

    void Update()
    {
        /*if (triggerCollider != null && isColliding)
        {
            isOn = triggerCollider.tag == "Conductive" && triggerCollider.GetComponent<MeshRenderer>().material.color == transform.parent.GetComponent<MeshRenderer>().material.color;
            isColliding = false;
        }
        else if (isCollidingWithLaser)
        {
            isOn = true;
        }
        else
        isOn = false;*/
        
        isOn = triggerCollider != null;
        
    }
}
