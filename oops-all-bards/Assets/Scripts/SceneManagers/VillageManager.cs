using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : BaseSceneManager
{
    private static GameObject playerModel;
    private static GameObject quintonModel;

    void Awake()
    {
        EntitySpawner.Instance.SpawnPlayer(playerSpawnPoint, playerCamera);
        base.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        QuestManager.Instance.UpdateQuestUI();
        //AudioManager.Instance.PlayMusicTrack("thehauntedhearth");
        playerModel = GameObject.FindGameObjectWithTag("Player");
        quintonModel = GameObject.Find("Quinton");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
