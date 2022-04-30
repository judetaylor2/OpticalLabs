using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity;
    public Transform playerBody;

    float xRotation;

    public bool isHoldingObject;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        //clamp camera viewing angle when holding object to avoid object getting to close to player
        if (!isHoldingObject)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        else
        xRotation = Mathf.Clamp(xRotation, -30f, 30f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //rotate the player in the camera script since it will always be the same as the mouseX
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
