using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

// A class that manages the features of the combat demo.
public class DemoManager : MonoBehaviour
{
    private static DemoManager _instance;
    public static DemoManager Instance => DemoManager._instance;
    public JSONReader jsonReader;
    public GameObject signpostContainer;
    public GameObject signpostPrefab;

    // Some constant strings for demo use/gameplay guidance only
    public const string help1 = "Welcome to the Oops! All Bards demo. You can control your character using WASD, and shift the camera using Q/E.";
    public const string help2 = "Check the top right corner of your screen for your current quest. You can interact with certain characters, like the innkeep, by pressing F.";
    public const string help3 = "Quinton has the Sardonic trait. Something you said resonated with him, and this has increased his affinity towards you.";
    public const string help4 = "Affinity is a measure of relationship recorded by CiF. Other data recorded by CiF include statuses and traits.";
    public const string help5 = "Quinton has joined your Band! You can check the members of your Band, their statuses, traits, and affinities by pressing B.";


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
        CreateSignpostMessage(help2);
        CreateSignpostMessage(help1);
        // Add player to the party.
        PartyManager.Instance.AddCharacterToParty(DataManager.Instance.PlayerData);
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

    public void RecruitQuinton()
    {
        // Recruit Quinton to the party.
        BasePlayer quinton = jsonReader.allies.GetBasePlayerByID(1);
        quinton.PlayerClass = jsonReader.baseClasses.baseClasses[2];
        quinton.Model = Resources.Load<GameObject>("PolygonVikings/Prefabs/Characters/Character_Chief_01_White");
        quinton.CiFData = new CiFData();
        quinton.CiFData.AddTrait(new Trait("Sardonic", 0));
        quinton.CiFData.AddTrait(new Trait("Vengeful", 1));
        quinton.CiFData.AddAffinity(new Affinity(0,5));
        PartyManager.Instance.AddCharacterToParty(quinton);
        CreateSignpostMessage(help5);
    }

    public void DestroySignpostMessage(GameObject toDestroy)
    {
        foreach (Transform child in signpostContainer.transform)
        {
            if (child.gameObject == toDestroy)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void CreateSignpostMessage(string text)
    {
        GameObject toInstantiate = Instantiate(signpostPrefab, signpostContainer.transform.position, Quaternion.identity);
        toInstantiate.transform.SetParent(signpostContainer.transform, true);
        toInstantiate.GetComponentInChildren<TMP_Text>().text = text;
        toInstantiate.GetComponentInChildren<Button>().onClick.AddListener(delegate { DestroySignpostMessage(toInstantiate); });
    }
}
