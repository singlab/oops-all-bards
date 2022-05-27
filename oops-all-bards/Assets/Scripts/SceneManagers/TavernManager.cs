using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernManager : MonoBehaviour
{
    public GameObject playerSpawnPoint;
    public CameraController playerCamera;
    private static GameObject playerModel;
    private static GameObject quintonModel;

    void Awake()
    {
        SpawnPlayer();
        AudioManager.Instance.ChangeTrack(1);
        CheckDialogueUI();
        TCPTestClient.Instance.RefreshWMEs();
    }

    // Update is called once per frame
    void Start()
    {
        playerModel = GameObject.FindGameObjectWithTag("Player");
        quintonModel = GameObject.Find("Quinton");
    }

    void SpawnPlayer()
    {
        // Instantiate chosen player model at spawn point.
        BasePlayer playerData = DataManager.Instance.PlayerData;
        GameObject toInstantiate = Instantiate(playerData.Model, playerSpawnPoint.transform.position, Quaternion.identity);
        // Set up rigidbody and collider for physics.
        Rigidbody rb = toInstantiate.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        // Set up player controller script and camera follow.
        PlayerController playerController = toInstantiate.AddComponent<PlayerController>();
        playerCamera.Player = toInstantiate;
        // Set up player animator controller.
        Animator a = toInstantiate.GetComponent<Animator>();
        a.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Controllers/Player");
        // Add player tag.
        toInstantiate.tag = "Player";
    }

    void CheckDialogueUI()
    {
        DialogueManager.Instance.AssignDialogueUI();
        if (DialogueManager.Instance.dialogueUI.activeSelf)
        {
            DialogueManager.Instance.ToggleDialogueUI();
        }
    }

    public static GameObject PlayerModel
    {
        get { return playerModel; }
    }

    public static GameObject QuintonModel
    {
        get { return quintonModel; }
    }
}
