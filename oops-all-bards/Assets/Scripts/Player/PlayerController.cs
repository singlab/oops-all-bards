using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    public float speed = 1;

    void Start()
    {
    	player = gameObject;
    }
 
    void Update()
    {
        Vector3 forward = player.transform.forward;
        Vector3 right = player.transform.right;

        // Move Forwards
        if (Input.GetKey(KeyCode.W)) 
        {
            player.transform.position += forward * speed * Time.deltaTime;
        }

        // // Move Backwards
        // if (Input.GetKey(KeyCode.S))
        // {
        //     // Adding a negative to the direction reverses it
        //     rb.AddForce (-ForwardDirection * movementIntensity);
        // }
        // // Move Rightwards (eg Strafe. *We are using A & D to swivel)
        // if (Input.GetKey(KeyCode.E))
        // {
        //    rb.AddForce (RightDirection * movementIntensity);
        // }
        // // Move Leftwards
        // if (Input.GetKey(KeyCode.Q))
        // {
        //    rb.AddForce (-RightDirection * movementIntensity);
        // }
    }
}
