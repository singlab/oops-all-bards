using UnityEngine;

public class OAB_KeyBoard_Mouse_Control : MonoBehaviour
{
    // Inspector
    public GameObject Player
    {
        get { return player; }
        set { player = value; }
    }
    public float Sensitivity {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    // For Player Control Toggle
    public bool _enable = true;
    // Mouse Sensitivity
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    // Private Field
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";
    
    private GameObject player;

    void Update(){
        if(_enable)
            MouseControl();
    }

    private void MouseControl()
    {
        // Align to camera target position
        transform.position = player.transform.Find("CameraTarget").position;
        
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        Quaternion localRotation = xQuat * yQuat;
        Quaternion playerLocalRotation = player.transform.localRotation;
        transform.localRotation = localRotation;
        player.transform.localRotation = new Quaternion(playerLocalRotation.x, localRotation.y,
            playerLocalRotation.z, playerLocalRotation.w);
    }
}