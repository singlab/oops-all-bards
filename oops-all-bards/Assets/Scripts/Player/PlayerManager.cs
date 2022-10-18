using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    // Inspector
    public ControlType controlType;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private CameraController playerCamera;
    
    // Private Field
    private static GameObject _playerModel;
    private UnityEvent<ControlType> controlTypeChange;
    private ControlType currentControlType;
    
    private void Start()
    {
        _playerModel = GameObject.FindGameObjectWithTag("Player");
        currentControlType = controlType;
        controlTypeChange = new UnityEvent<ControlType>();
    }

    private void Update()
    {
        // Check Current Control Type
        if (currentControlType != controlType)
        {
            controlTypeChange.Invoke(controlType);
            currentControlType = controlType;
        }
    }

    // Track Subscribers
    // Preventing Subscription By Mistake
    public void SubscribeControlTypeChange(UnityAction<ControlType> typeChange)
    {
        controlTypeChange.AddListener(typeChange);
        currentControlType = controlType;
        controlTypeChange.Invoke(controlType);  // Update current control type
    }
    
    public enum ControlType
    {
        KEYBOARD_ONLY,
        KEYBOARD_MOUSE
    }
    
    public GameObject SpawnPlayer()
    {
        // Instantiate chosen player model at spawn point.
        BasePlayer playerData = DataManager.Instance.PlayerData;
        GameObject toInstantiate = Instantiate(playerData.Model, playerSpawnPoint.transform.position, Quaternion.identity);
        // Set up rigidbody and collider for physics.
        Rigidbody rb = toInstantiate.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        // Set up player controller script and camera follow.
        PlayerController playerController = toInstantiate.AddComponent<PlayerController>();
        playerCamera.CameraLinkPlayer(toInstantiate);
        // Set up player animator controller.
        Animator a = toInstantiate.GetComponent<Animator>();
        a.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/Player");
        // Add player tag.
        toInstantiate.tag = "Player";
        return _playerModel;
    }
    
    
}
