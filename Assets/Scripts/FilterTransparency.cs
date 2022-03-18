using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterTransparency : MonoBehaviour
{
    Material m;

    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<MeshRenderer>().material;    
    }

    // Update is called once per frame
    void Update()
    {
        Material m1 = m;
        m1.color = new Color(m.color.r, m.color.g, m.color.b, 111);
        m = m1;
        Debug.Log("hello");
    }
}
