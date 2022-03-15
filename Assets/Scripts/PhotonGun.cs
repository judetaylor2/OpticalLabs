using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonGun : MonoBehaviour
{
    public Color[] colours;
    public Transform raycastStartPoint;
    public float photonDistance;
    int currentColourIndex;
    public Image colorImage;
    public LayerMask groundMask;
    
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
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance, Physics.AllLayers))
            {
                if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 9 || hit.transform.gameObject.layer == 10 || hit.transform.tag == "Filter")
                {
                    Material m = hit.transform.GetComponent<MeshRenderer>().material;
                    m.color = colours[currentColourIndex];
                    hit.transform.GetChild(1).GetComponent<MeshRenderer>().material = m;
                    
                    if (hit.transform.gameObject.layer == 10)
                    if (currentColourIndex == 0)
                    {
                        //speedCollider.transform.localScale = hit.transform.localScale;
                        //Instantiate(speedCollider, hit.transform.position, hit.transform.rotation);

                        hit.transform.tag = "Speed";
                    }
                    else if (currentColourIndex == 1)
                    {
                        hit.transform.tag = "Gravity";
                    }
                    else if (currentColourIndex == 2)
                    {
                        hit.transform.tag = "Bounce";
                    }                    
                    
                }
                
            }
            
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastStartPoint.position, raycastStartPoint.forward, out hit, photonDistance))
            if (hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 9 || hit.transform.gameObject.layer == 10 || hit.transform.tag == "Filter")
            {
                Material m = hit.transform.GetComponent<MeshRenderer>().material;
                m.color = Color.white;
                hit.transform.GetChild(1).GetComponent<MeshRenderer>().material = m;

                hit.transform.tag = "Untagged";
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
