using UnityEngine;

public class OAB_KeyBoardControl : MonoBehaviour
{
    private Vector3 left = new Vector3(0, -1, 0);
    private Vector3 right = new Vector3(0, 1, 0);
    private GameObject player;
    public float speed = 100;
    public bool _enable = true;

    // Update is called after Update each frame
    void Update () 
    {
        if(_enable)
            InputControl();
    }

    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }

    private void InputControl()
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
    }

}
