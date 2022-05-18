using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A class that manages the features of the combat demo.
public class DemoManager : MonoBehaviour
{
    private static DemoManager _instance;
    public static DemoManager Instance => DemoManager._instance;
    public JSONReader jsonReader;

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
        // Subscribe to events that the demo manager should be aware of.
        SubscribeToEvents();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<BaseEnemy> GenerateEnemies()
    {
        List<BaseEnemy> enemies = new List<BaseEnemy>();
        BaseEnemy enemy = new BaseEnemy("Devotee", 10, 5, 0, jsonReader.baseClasses.GetRandomClass());
        enemies.Add(enemy);
        enemy = new BaseEnemy("Fanatic", 10, 5, 0, jsonReader.baseClasses.GetRandomClass());
        enemies.Add(enemy);
        return enemies;
    }

    // A function used to debug the player object.
    private void DebugPlayer(BasePlayer player)
    {
        Debug.Log(player.Name);
        Debug.Log("FAME: " + player.Fame);
        Debug.Log("GOLD: " + player.Gold);
        Debug.Log(player.PlayerClass.Name);
        foreach (BaseAbility ability in player.PlayerClass.Abilities)
        {
            Debug.Log(ability.Name + " " + ability.Damage + " " + ability.Cost);
        }
    }

    // A function that uses the event management system to subscribe to events used in this manager.
    private void SubscribeToEvents()
    {
        EventManager.Instance.SubscribeToEvent(EventType.CheckQueue, CheckQueue);
        EventManager.Instance.SubscribeToEvent(EventType.AwaitPlayerInput, AwaitPlayerInput);
        EventManager.Instance.SubscribeToEvent(EventType.CombatLoss, CombatLoss);
        EventManager.Instance.SubscribeToEvent(EventType.CombatWin, CombatWin);
    }

    public void CheckQueue()
    {
        // Refresh WMEs every time we check the queue.
        TCPTestClient.Instance.RefreshWMEs();
        if (!CombatManager.Instance.combatQueue.IsEmpty())
        {
            ICombatQueueable cq = CombatManager.Instance.combatQueue.Pop();
            cq.Execute();
        } else 
        {
            Debug.Log("Combat round has ended. Resetting queue.");
            CombatManager.Instance.rounds += 1;
            CombatManager.Instance.InitCombatQueue(CombatManager.Instance.party, CombatManager.Instance.enemies);
        }
    }

    // A function that enables player input when it is the player's turn in the combat queue.
    private void AwaitPlayerInput()
    {
        Debug.Log("Awaiting player input...");
    }

    public void CombatWin()
    {
        PartyManager.Instance.ToggleInCombat(false);
        LoadScene("CombatWin");
    }

    public void CombatLoss()
    {
        PartyManager.Instance.ToggleInCombat(false);
        LoadScene("CombatLoss");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TogglePlayerControls()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Camera camera = Camera.main;
        player.GetComponent<PlayerController>().ToggleControls();
        camera.GetComponent<CameraController>().ToggleControls();
    }

    public void PrepareForGig()
    {
        // Add player to the party.
        PartyManager.Instance.AddCharacterToParty(DataManager.Instance.PlayerData);
        // Recruit Quinton to the party.
        BasePlayer quinton = jsonReader.allies.GetBasePlayerByID(1);
        quinton.PlayerClass = jsonReader.baseClasses.baseClasses[2];
        PartyManager.Instance.AddCharacterToParty(quinton);
        // Load the combat scene.
        LoadScene("GigDemo");
    }
}
