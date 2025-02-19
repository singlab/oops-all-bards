using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes objects float up & down while gently spinning.
public class Floater : MonoBehaviour {
    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.1f;
    public float frequency = 1f;
 
    // Position Storage Variables
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
    private Camera mainCamera;
 
    // Use this for initialization
    void Start () {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        mainCamera = Camera.main;
    }
     
    // Update is called once per frame
    void Update () {
        // Spin object around Y-Axis
        // transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
        tempPos.x = transform.position.x;
        tempPos.z = transform.position.z;
 
        transform.position = tempPos;

        // Face the camera
        if (mainCamera != null) // Check if the camera exists
        {
            Vector3 directionToCamera = mainCamera.transform.position - transform.position;
            directionToCamera.y = 0; // Keep the object upright (optional)

            if (directionToCamera != Vector3.zero) // Avoids errors if the object and camera are at the same position
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1);
            }
        }
    }
}
