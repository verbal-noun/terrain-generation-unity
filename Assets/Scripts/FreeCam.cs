using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float speed = 10f;
    public Camera cam;
    public float rotateSpeed = 2.5f;
    private float xRotation;
    private float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) {
            cam.transform.position = cam.transform.position + (-1) * cam.transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            cam.transform.position = cam.transform.position + cam.transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W)) {
            cam.transform.position = cam.transform.position + cam.transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            cam.transform.position = cam.transform.position + (-1) * cam.transform.forward * speed * Time.deltaTime;
        }

        xRotation += Input.GetAxis("Mouse X") * rotateSpeed;
        yRotation += Input.GetAxis("Mouse Y") * rotateSpeed;
        cam.transform.rotation = Quaternion.Euler(-yRotation, xRotation, 0);
    }

    void OnCollisionEnter () {
        print("Hit");
    }
}
