using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [HideInInspector] public bool isColliding;
    public GameObject laserObject;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            laserObject.SetActive(true);
        }
        else
        {
            laserObject.SetActive(false);
        }
        
        isColliding = false;
    }
}
