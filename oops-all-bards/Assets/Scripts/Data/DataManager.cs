using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private BasePlayer playerData;
    private static DataManager _instance;
    public static DataManager Instance => DataManager._instance;

    // Singleton pattern
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BasePlayer PlayerData
    {
        get { return this.playerData; }
        set { this.playerData = value; }
    }

    public void SavePlayerData()
    {
        string playerDataJson = JsonUtility.ToJson(this.playerData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData.json", playerDataJson);
    }
}
