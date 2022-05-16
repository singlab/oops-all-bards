using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        // offset = transform.position - player.transform.position;
    }

    // Update is called after Update each frame
    void Update () 
    {
        // Align to player position
        transform.position = player.transform.position;
        
        // Rotate left
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * 100, Space.World);
        }
        
        // Rotate right
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 100, Space.World);
        }

        // // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        // transform.position = player.transform.position + offset;
    }

    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }
}
