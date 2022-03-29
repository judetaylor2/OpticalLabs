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
        //isColliding = true;
        
        isOn = ((other.gameObject.layer == 8 || other.gameObject.layer == 9) && other.transform.GetChild(1).GetComponent<MeshRenderer>().material.color == transform.GetChild(1).GetComponent<MeshRenderer>().material.color);
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.layer == 8 || other.gameObject.layer == 9) && !isCollidingWithLaser)
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
        
        //isOn = triggerCollider != null;
        
        isOn = isCollidingWithLaser;
    }
}
