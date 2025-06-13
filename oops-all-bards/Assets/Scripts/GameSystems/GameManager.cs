using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

// A class that manages the features of the combat demo.
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => GameManager._instance;
    public int tavernVisits = 1;
    public bool completedGame = false;

    // Public/private fields for combat scene management
    public string previousSceneName;
    public Vector3 playerPositionBeforeCombat;
    public Quaternion playerRotationBeforeCombat;
    public List<GameObject> combatants_playerParty = new List<GameObject>();
    public List<string> combatants_enemyParty = new List<string>();
    private List<PersistentCharacterState> charactersInSceneBeforeCombat = new List<PersistentCharacterState>();

    // private fields for event management
    private Action<object> checkQueueLambda;
    private Action<object> awaitPlayerInputLambda;
    private Action<object> combatLossLambda;
    private Action<object> combatWinLambda;

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
        EventManager.Instance.SubscribeToEvent(EventType.OnInteraction, HandleCombatStartInteraction);

        // Add player to the party.
        PartyManager.Instance.AddCharacterToParty(DataManager.Instance.PlayerData);
        // TCPTestClient.Instance.RefreshWMEs();
    }

    // Update is called once per frame
    void Update()
    {

        if (completedGame)
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
        // Create lambdas for the events that this manager should be aware of
        checkQueueLambda = (eventData) => CheckQueue();
        awaitPlayerInputLambda = (eventData) => AwaitPlayerInput();
        combatLossLambda = (eventData) => CombatLoss();
        combatWinLambda = (eventData) => CombatWin();

        EventManager.Instance.SubscribeToEvent(EventType.CheckQueue, checkQueueLambda);
        EventManager.Instance.SubscribeToEvent(EventType.AwaitPlayerInput, awaitPlayerInputLambda);
        EventManager.Instance.SubscribeToEvent(EventType.CombatLoss, combatLossLambda);
        EventManager.Instance.SubscribeToEvent(EventType.CombatWin, combatWinLambda);
    }

    public void CheckQueue()
    {
        // Refresh WMEs every time we check the queue.
        TCPTestClient.Instance.RefreshWMEs();
        if (!CombatManager.Instance.combatQueue.IsEmpty())
        {
            ICombatQueueable cq = CombatManager.Instance.combatQueue.Pop();
            cq.Execute();
            if (CombatManager.Instance.combatQueue.queue.Count < CombatUI.Instance.queueDisplay.Count)
            {
                CombatUI.Instance.RenderPop();
            }
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
        DemoManager.Instance.LoadScene("CombatWin");
    }

    public void CombatLoss()
    {
        PartyManager.Instance.ToggleInCombat(false);
        DemoManager.Instance.LoadScene("CombatLoss");
    }

    public void TogglePlayerControls()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Camera camera = Camera.main;
        player.GetComponent<PlayerController>().ToggleControls();
        camera.GetComponent<CameraController>().ToggleControls();
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

    void OnDestroy()
    {
        if (_instance == this) // Only if this is the true instance
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.UnsubscribeToEvent(EventType.CheckQueue, checkQueueLambda);
                EventManager.Instance.UnsubscribeToEvent(EventType.AwaitPlayerInput, awaitPlayerInputLambda);
                EventManager.Instance.UnsubscribeToEvent(EventType.CombatLoss, combatLossLambda);
                EventManager.Instance.UnsubscribeToEvent(EventType.CombatWin, combatWinLambda);
                EventManager.Instance.UnsubscribeToEvent(EventType.OnInteraction, HandleCombatStartInteraction);
            }
        }
    }

    private void HandleCombatStartInteraction(object eventData)
    {
        Dictionary<string, object> data = eventData as Dictionary<string, object>;
        if (data == null) return;

        string interactionType = data.TryGetValue("interactionType", out object typeObj) ? typeObj as string : null;
        string outcome = data.TryGetValue("outcome", out object outcomeObj) ? outcomeObj as string : null;

        // Check if this is one of our defined combat start outcomes
        if (interactionType == InteractionTypes.Combat &&
            (outcome == OutcomeStrings.Combat.Combat_PlayerInitiated_Piggy ||
             outcome == OutcomeStrings.Combat.Combat_PlayerInitiated_QuestNPC ||
             outcome == OutcomeStrings.Combat.Combat_WurguthAttacksPlayer ||
             outcome == OutcomeStrings.Combat.Combat_LocationTrigger_Quinton))
        {
            GameObject eventActor = data.TryGetValue("actor", out object actorObj) ? actorObj as GameObject : null;
            GameObject eventTarget = data.TryGetValue("target", out object targetObj) ? targetObj as GameObject : null;

            if (eventActor != null && eventTarget != null)
            {
                Debug.Log($"HandleCombatStartInteraction: Player ({eventActor.name}) is initiating combat with ({eventTarget.name}) based on outcome: {outcome}");

                // Determine who is player and who is enemy for this specific trigger
                GameObject playerCombatant = null;
                string enemyCombatant = null;

                if (eventActor.CompareTag("Player")) // Assuming Player is the actor initiating
                {
                    playerCombatant = eventActor;
                    enemyCombatant = eventTarget.name;
                }
                else if (eventTarget.CompareTag("Player")) // Could be that an NPC (actor) attacks Player (target)
                {
                    playerCombatant = eventTarget;
                    enemyCombatant = eventActor.name;
                }
                else
                {
                    Debug.LogError("CombatStartInteraction: Could not determine Player from event actor/target.");
                    return;
                }

                // HACK: If the Combat_LocationTrigger_Quinton outcome is used, set enemyCombatant to null to allow CombatManager to spawn random enemies.
                if (outcome == OutcomeStrings.Combat.Combat_LocationTrigger_Quinton)
                {
                    enemyCombatant = null;
                }

                StartCombatEncounter(new List<GameObject> { playerCombatant }, new List<string> { enemyCombatant });
            }
        }
    }

    public void StartCombatEncounter(List<GameObject> playerUnits, List<string> enemyUnits)
    {
        charactersInSceneBeforeCombat.Clear();

        IEnumerable<Viv.VivCharacter> allVivCharacters = Viv.Viv.Instance.GetAllRegisteredCharacters();

        Debug.Log($"Found {((List<Viv.VivCharacter>)allVivCharacters).Count} VivCharacters to save before combat.");

        foreach (Viv.VivCharacter character in allVivCharacters)
        {
            charactersInSceneBeforeCombat.Add(new PersistentCharacterState(character, SpawnMethod.Instant));
        }

        if (playerUnits == null || playerUnits.Count == 0 || enemyUnits == null || enemyUnits.Count == 0)
        {
            Debug.LogError("StartCombatEncounter called with empty player or enemy units.");
            return;
        }

        // Clear previous combatants
        combatants_playerParty.Clear();
        combatants_enemyParty.Clear();

        // Store the GameObjects for the Combat Scene to use
        foreach (var playerUnit in playerUnits)
        {
            if (playerUnit != null) combatants_playerParty.Add(playerUnit);
        }
        foreach (var enemyUnit in enemyUnits)
        {
            if (enemyUnit != null) combatants_enemyParty.Add(enemyUnit);
        }

        // Disable characters in the current scene if they are being "moved"
        // This depends on how your combat scene works (does it spawn new instances or use existing ones?)
        // foreach(var p in combatants_playerParty) p.SetActive(false);
        // foreach(var e in combatants_enemyParty) e.SetActive(false);

        Debug.Log($"Preparing to start combat. Player Party Count: {combatants_playerParty.Count}, Enemy Party Count: {combatants_enemyParty.Count}");

        // Set previous scene information
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerPositionBeforeCombat = player.transform.position;
        playerRotationBeforeCombat = player.transform.rotation;
        previousSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Previous Scene: {previousSceneName}, Player Position: {playerPositionBeforeCombat}, Player Rotation: {playerRotationBeforeCombat}");

        // Load the combat scene
        SceneManager.LoadScene("GigDemo");
    }

    public void ReturnToWorldAfterCombat()
    {
        if (string.IsNullOrEmpty(previousSceneName))
        {
            Debug.LogError("ReturnToWorldAfterCombat called without a previous scene.");
            return;
        }
        StartCoroutine(LoadPreviousSceneAndRepositionPlayer());
    }

    IEnumerator LoadPreviousSceneAndRepositionPlayer()
    {
        // Load the previous scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(previousSceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        var spawnedCharacters = new List<Viv.VivCharacter>();
        var characterStates = new List<PersistentCharacterState>(charactersInSceneBeforeCombat);
        charactersInSceneBeforeCombat.Clear();

        Debug.Log("Spawner Pass 1: Instantiating all characters...");
        foreach (var charState in characterStates)
        {
            GameObject prefab = Resources.Load<GameObject>(charState.prefabResourcePath);
            if (prefab != null)
            {
                GameObject newCharGO = Instantiate(prefab, charState.lastPosition, charState.lastRotation);
                Viv.VivCharacter newChar = newCharGO.GetComponent<Viv.VivCharacter>();
                if (newChar != null)
                {
                    // Add the newly created character to our temporary list
                    spawnedCharacters.Add(newChar);
                }
            }
        }

        yield return null;

        Debug.Log("Spawner Pass 2: Initializing state for all characters...");
        for (int i = 0; i < spawnedCharacters.Count; i++)
        {
            spawnedCharacters[i].InitializeFromState(characterStates[i]);
        }

        Debug.Log($"Returning to world. Re-spawning {charactersInSceneBeforeCombat.Count} VivCharacters.");

        foreach (var charState in charactersInSceneBeforeCombat)
        {
            GameObject prefab = Resources.Load<GameObject>(charState.prefabResourcePath);
            if (prefab != null)
            {
                GameObject newCharGO = Instantiate(prefab, charState.lastPosition, charState.lastRotation);
                Viv.VivCharacter newChar = newCharGO.GetComponent<Viv.VivCharacter>();

                if (newChar != null)
                {
                    newChar.InitializeFromState(charState);
                }
            }
        }
        // Clear the list now that we're done with it.
        charactersInSceneBeforeCombat.Clear();

        // Reposition the player
        TogglePlayerControls();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = playerPositionBeforeCombat;
            player.transform.rotation = playerRotationBeforeCombat;
            Debug.Log($"Player repositioned to: {player.transform.position}, Rotation: {player.transform.rotation}");
        }

        // Re-enable player controls
        TogglePlayerControls();
        previousSceneName = string.Empty; // Clear the previous scene name
        playerPositionBeforeCombat = Vector3.zero; // Reset player position
        playerRotationBeforeCombat = Quaternion.identity; // Reset player rotation
        Debug.Log("Player controls re-enabled and combat scene unloaded.");
        // Check if fame thresholds were crossed AFTER reloading
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        FameManager.Instance.CheckAndTriggerFameThresholds(playerObject, FameManager.Instance.OldFame, FameManager.Instance.CurrentPlayerFame);
    }
}
