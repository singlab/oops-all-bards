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
    public GameObject firstFightTrigger;

    public int tavernVisits = 1;
    public bool hasAssistedOnce = false;
    public bool hasBeenProtectedOnce = false;
    public bool hasRequestAidOnce = false;
    public bool completedDemo = false;

    // Some constant strings for demo use/gameplay guidance only
    public const string help1 = "Welcome to the Oops! All Bards demo. You can control your character using WASD, and shift the camera using Q/E.";
    public const string help2 = "Check the top right corner of your screen for your current quest. You can interact with certain characters, like the innkeep, by pressing F.";
    public const string help3 = "Quinton has the Sardonic trait. Something you said resonated with him, and this has increased his affinity towards you.";
    public const string help4 = "Affinity is a measure of relationship recorded by CiF. Other data recorded by CiF include statuses and traits.";
    public const string help5 = "Quinton has joined your Band! You can check the members of your Band, their statuses, traits, and affinities by pressing B.";
    public const string help6 = "During combat, your allies like Quinton will often act according to their current CiF status. Quinton has just decided to PROTECT you from the next enemy attack headed your way!";
    public const string help7 = "Allies like Quinton will also react to their immediate context. Quinton has just called out for help! Your response, or lack of one, will impact his CiF status and, possibly, long-term behaviors.";
    public const string help8 = "Uh-oh. Looks like Quinton isn't too happy with how things went down. Maybe you can still salvage things?";
    public const string help9 = "This concludes the demo of Oops! All Bards. You can look forward to expansions of this work in terms of AI, gameplay systems, and game content. You can press ESC to quit to desktop. Until next time, brave bard!";


    // Singleton pattern
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != null)
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
        TCPTestClient.Instance.RefreshWMEs();
        // Prevent the first fight being able to trigger without quinton in the party
        firstFightTrigger.SetActive(false);

       
    }

    // Update is called once per frame
    void Update()
    {
      
        if (completedDemo)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
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
        }
        else
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
        // SceneManager.LoadScene(sceneName);
        Destroy(firstFightTrigger); //prevents after fight bug
        BlackFade fader = GameObject.Find("BlackFade").GetComponent<BlackFade>();
        fader.FadeToLevel(sceneName);
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
        quinton.CiFData.AddTrait(new Trait(Trait.TraitTypes.SARDONIC));
        quinton.CiFData.AddTrait(new Trait(Trait.TraitTypes.VENGEFUL));
        quinton.CiFData.AddAffinity(new Affinity(0, 5));
        PartyManager.Instance.AddCharacterToParty(quinton);
        TCPTestClient.Instance.RefreshWMEs();
        CreateSignpostMessage(help5);
        //once quinton is in the party, let the next scene be able to be triggered
        firstFightTrigger.SetActive(true);
    }

    public void DestroySignpostMessage(GameObject toDestroy)
    {
        foreach (Transform child in signpostContainer.transform)
        {
            if (child.gameObject == toDestroy)
            {
                Destroy(child.gameObject);
                
            }

            //Locks cursor when help messages are not shown
            //Code also prevent premature locking if a help message is displayed during dialogue
            if(GameObject.Find("SignpostContainer").transform.childCount == 1 && !DialogueManager.Instance.dialogueUI.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.Locked;
                TogglePlayerControls();
            }
        }
    }

    public void CreateSignpostMessage(string text)
    {
        //StartCoroutine(togglePlayerPause());

        if (signpostContainer == null)
        { signpostContainer = GameObject.Find("SignpostContainer"); }
        GameObject toInstantiate = Instantiate(signpostPrefab, signpostContainer.transform.position, Quaternion.identity);
        toInstantiate.transform.SetParent(signpostContainer.transform, false); 
        toInstantiate.transform.position = toInstantiate.transform.parent.position;
        toInstantiate.GetComponentInChildren<TMP_Text>().text = text;
        StartCoroutine(togglePlayerPause());
        toInstantiate.GetComponentInChildren<Button>().onClick.AddListener(delegate { DestroySignpostMessage(toInstantiate); });
    }

    public static IEnumerator togglePlayerPause()
    {
        //Delay necessary to prevent cursor from being locked too soon between start of level and 1st help message 
        yield return new WaitForSeconds(0.003f);
        //Code enables cursor to be used to close the signpost message
        Cursor.lockState = CursorLockMode.Confined;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CameraController>().enabled = false;
    }

    
    public void IncrementTavernVisits()
    {
        tavernVisits++;
    }

    

}
