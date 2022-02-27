using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonGun : MonoBehaviour
{
    public Color[] colours;
    public Transform cameraPoint;
    public float photonDistance;
    int currentColourIndex;
    public Image colorImage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraPoint.position, cameraPoint.forward, out hit, photonDistance))
            if (hit.transform.tag == "Conductive")
            {
                Material m = hit.transform.GetComponent<MeshRenderer>().material;
                m.color = colours[currentColourIndex];
                hit.transform.GetComponent<MeshRenderer>().material = m;
            }
            
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraPoint.position, cameraPoint.forward, out hit, photonDistance))
            if (hit.transform.tag == "Conductive")
            {
                Material m = hit.transform.GetComponent<MeshRenderer>().material;
                m.color = Color.white;
                hit.transform.GetComponent<MeshRenderer>().material = m;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentColourIndex == colours.Length - 1)
            currentColourIndex = 0;
            else 
            currentColourIndex++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentColourIndex == 0)
            currentColourIndex = colours.Length - 1;
            else 
            currentColourIndex--;
        }

        Color c = colours[currentColourIndex];
        c.a = 1;
        colorImage.color = c; 
    }
}
