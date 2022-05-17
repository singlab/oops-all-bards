using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 left = new Vector3(0, -1, 0);
    private Vector3 right = new Vector3(0, 1, 0);
    private GameObject player;
    public float speed = 100;

    // Start is called before the first frame update
    void Start()
    {
        // //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        // offset = transform.position - player.transform.position;
    }

    // Update is called after Update each frame
    void Update () 
    {
        // Align to camera target position
        transform.position = player.transform.Find("CameraTarget").position;
        
        // Rotate left
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(left * Time.deltaTime * speed, Space.World);
            player.transform.Rotate(left * Time.deltaTime * speed, Space.World);
        }
        
        // Rotate right
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(right * Time.deltaTime * speed, Space.World);
            player.transform.Rotate(right * Time.deltaTime * speed, Space.World);
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
