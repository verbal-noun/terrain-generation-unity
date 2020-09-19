using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for camera to handle movement and viewing directions using keyboard
// and mouse
public class FreeCam : MonoBehaviour
{
    public float speed = 25f;
    public float terrainSize = 128f;
    public float startHeight = 40f;
    public CharacterController controller;
    public float rotateSpeed = 1f;
    private float xRotation;
    private float yRotation;
    private Camera cam;

    // Assigning the camera
    void Start()
    {
        cam = Camera.main;
        // prevents cursor from moving off screen
        Cursor.lockState = CursorLockMode.Locked;
        // set suitable initial position and viewing direction of camera
        cam.transform.position = new Vector3(terrainSize/2, startHeight, terrainSize/2);
        cam.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {   
        // Movement of camera on its relative x and z axis
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = cam.transform.right*x+cam.transform.forward*z;
        controller.Move(move*speed*Time.deltaTime);
        
        // Rotation of camera
        xRotation += Input.GetAxis("Mouse X") * rotateSpeed;
        zRotation += Input.GetAxis("Mouse Y") * rotateSpeed;
        cam.transform.rotation = Quaternion.Euler(-zRotation, xRotation, 0);

        // reset the position and view of the camera when space is pressed
        if (Input.GetKeyDown("space")) {
            cam.transform.position = new Vector3(terrainSize/2, startHeight, terrainSize/2);
            cam.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }
}
