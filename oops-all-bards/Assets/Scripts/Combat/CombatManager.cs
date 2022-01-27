using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CombatManager : MonoBehaviour
{

    private static CombatManager _instance;
    // A reference to the stage.
    GameObject stage;
    // A reference to the display area.
    GameObject display;
    // A reference to the gameobject queue.
    GameObject queueableContainer;
    // A reference to the target menu.
    GameObject targetMenu;
    // A reference to the queueable gameobject prefab.
    public GameObject goQueueable;
    // A reference to the target button prefab.
    public GameObject targetButton;
    // A reference to the combat queue.
    public CombatQueue combatQueue;
    // A reference to the player party.
    public List<BasePlayer> party = new List<BasePlayer>();
    // A reference to the enemies.
    public List<BaseEnemy> enemies = new List<BaseEnemy>();
    // A counter for the number of rounds combat has lasted for.
    public int rounds = 1;
    // A reference to a target name, if any.
    string target = null;
    public static CombatManager Instance => CombatManager._instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stage = GameObject.Find("Stage");
        display = GameObject.Find("Display");
        queueableContainer = GameObject.Find("CombatQueue");
        targetMenu = GameObject.Find("TargetMenu");

        stage.SetActive(false);
        display.SetActive(false);
        queueableContainer.SetActive(false);
        SubscribeToEvents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // A function that uses the event management system to subscribe to events used in this manager.
    private void SubscribeToEvents()
    {
        EventManager.Instance.SubscribeToEvent(EventType.EnemyAI, TakeEnemyAction);
    }

    // A function used to render all UI elements for the demo.
    public void RenderUI()
    {
        // Disable the init button.
        GameObject.Find("InitButton").SetActive(false);

        // Render the placeholder stage and queueable container.
        stage.SetActive(true);
        queueableContainer.SetActive(true);
    }

    // A function used to render a particular character's combat input menu.
    public void RenderInputMenu(BasePlayer actingCharacter)
    {
        // Render the UI for the input.
        Debug.Log("Rendering input menu for " + actingCharacter.name + " now.");
        display.SetActive(true);
        GameObject activeMenu = display.transform.Find(actingCharacter.name + "Info").gameObject;
        activeMenu.SetActive(true);
        foreach (Transform child in display.transform)
        {
            if (child.gameObject.name != activeMenu.name)
            {
                child.gameObject.SetActive(false);
            }
        }
        // Fill in the name, health, and ability names of the acting character.
        GameObject nameplate = activeMenu.transform.GetChild(0).GetChild(0).Find("Name").gameObject;
        nameplate.GetComponent<Text>().text = actingCharacter.name;
        GameObject health = activeMenu.transform.GetChild(0).GetChild(0).Find("Health").gameObject;
        health.GetComponent<Text>().text = actingCharacter.health.ToString();
        // TODO: This is pretty hacky, should probably go by number of abilities available to player and then make button prefabs/place them in menu.
        for (int i = 1; i < 4; i++)
        {
            // Update the button text.
            GameObject abilityButton = activeMenu.transform.GetChild(0).GetChild(0).Find("Ability" + i).gameObject;
            BaseAbility currentAbility = actingCharacter.playerClass.abilities[i-1];
            abilityButton.GetComponentInChildren<Text>().text = currentAbility.name;
            // Add on click functions to the buttons to create a PlayerAction queueable.
            abilityButton.GetComponent<Button>().onClick.AddListener(() => 
                {StartCoroutine(SelectTarget(currentAbility, actingCharacter));});
        }
    }

    // A function used to initialize the combat queue.
    public void InitCombatQueue(List<BasePlayer> party, List<BaseEnemy> enemies)
    {
        // Set private references to party and enemies.
        this.party = party;
        this.enemies = enemies;
        // Clean up for new combat scenario.
        combatQueue = new CombatQueue();
        combatQueue.Clear();
        ClearQueueUI();
        // Add standard functions for start, player input, enemy AI, and end.
        PushAndCreateCombatQueueable(new CombatStart());
        foreach (BasePlayer p in party)
        {
            PushAndCreateCombatQueueable(new PlayerTurn(p));
            // TODO: This needs to go elsewhere, preferably somewhere that has access to party and enemy lists.
            // Populate the target menu with buttons for each player character/enemy.
            if (rounds == 1)
            {
                GameObject toInstantiate = Instantiate(targetButton, targetMenu.transform.GetChild(0).transform.GetChild(0).transform);
                toInstantiate.GetComponentInChildren<Text>().text = p.name;
                toInstantiate.GetComponent<Button>().onClick.AddListener(() => {
                    target = p.name;
                });
            }
        }
        foreach (BaseEnemy e in enemies)
        {
            PushAndCreateCombatQueueable(new EnemyTurn(e));
            // TODO: This needs to go elsewhere, preferably somewhere that has access to party and enemy lists.
            // Populate the target menu with buttons for each player character/enemy.
            if (rounds == 1)
            {
                GameObject toInstantiate = Instantiate(targetButton, targetMenu.transform.GetChild(0).transform.GetChild(0).transform);
                toInstantiate.GetComponentInChildren<Text>().text = e.name;
                toInstantiate.GetComponent<Button>().onClick.AddListener(() => {
                    target = e.name;
                });
            }
        }
        // Hide the targetMenu.
        targetMenu.SetActive(false);
        // Flag the DemoManager to begin checking queue.
        EventManager.Instance.InvokeEvent(EventType.CheckQueue, null);
    }

    // A function used to push a CombatQueueable to the CombatQueue and create associated gameobject.
    public void PushAndCreateCombatQueueable(ICombatQueueable queueable)
    {
        // Push the queueable to the CombatQueue.
        combatQueue.Push(queueable);
        // Create gameobject and set container as parent.
        Instantiate(goQueueable, queueableContainer.transform.GetChild(0).transform);
    }

    // A function used to clear the queueable container UI of child gameobjects.
    public void ClearQueueUI()
    {
        for(int i = 0; i < queueableContainer.transform.GetChild(0).transform.childCount; i++)
        {
            GameObject go = queueableContainer.transform.GetChild(0).transform.GetChild(i).gameObject;
            Destroy(go);
        }
    }

    // A coroutine used to open the target menu after clicking on an ability button. Waits until target
    // is selected, closes target menu, and then creates a PlayerAction.
    IEnumerator SelectTarget(BaseAbility ability, BasePlayer actingCharacter)
    {
        ITargetable targetable = null;
        targetMenu.SetActive(true);
        target = null;
        while (target == null) { yield return null; }
        foreach (BasePlayer p in party)
        {
            if (target == p.name)
            {
                targetable = p;
            }
        }
        foreach (BaseEnemy e in enemies)
        {
            if (target == e.name)
            {
                targetable = e;
            }
        }
        AddPlayerAction(ability, actingCharacter, targetable);
        targetMenu.SetActive(false);
    }

    // A function used to remove a target button from the target menu.
    public void RemoveTargetButton(string name)
    {
        targetMenu.SetActive(true);
        for (int i = 0; i < targetMenu.transform.GetChild(0).transform.GetChild(0).transform.childCount; i++)
        {
            GameObject child = targetMenu.transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject;
            if (child.GetComponentInChildren<Text>().text == name)
            {
                Destroy(child);
            }
        }
        targetMenu.SetActive(false);
    }

    // A function used to create a PlayerAction queueable and push it to the front of the queue.
    public void AddPlayerAction(BaseAbility ability, BasePlayer actingCharacter, ITargetable target)
    {
        Debug.Log("Adding player action...");
        // Create the PlayerAction queueable object for each ability.
        PlayerAction action = new PlayerAction(ability, actingCharacter, target);
        // Push the PlayerAction to the front of the queue.
        combatQueue.PriorityPush(action);
        // Tell DemoManager to check the queue and complete the action.
        EventManager.Instance.InvokeEvent(EventType.CheckQueue, null);
    }

    // A function used to resolve/apply effects of a PlayerAction queueable.
    public void ResolvePlayerAction(PlayerAction action)
    {
        // TODO: Add flourish points/action economy. Remove the cost of ability in all cases.
        // action.actingCharacter.flourish -= action.ability.cost;
        if (action.ability.combatType == BaseAbility.CombatAbilityTypes.ATTACK)
        {
            action.target.health -= action.ability.damage;
            Debug.Log(action.actingCharacter.name + " deals " + action.ability.damage + " damage to " + action.target.name + ".");
            CheckCombatantsHealth(action.target);
        }
        if (action.ability.combatType == BaseAbility.CombatAbilityTypes.HEAL)
        {
            action.target.health += action.ability.damage;
            Debug.Log(action.actingCharacter.name + " heals " + action.target.name + " for " + action.ability.damage + " health!" );
        }
        if (action.ability.combatType == BaseAbility.CombatAbilityTypes.DEFEND)
        {
            action.target.shield += action.ability.damage;
            Debug.Log(action.actingCharacter.name + " is shielding " + action.target.name + " for " + action.ability.damage + " damage." );
        }
        // TODO: Update the UI to reflect new values.

        // Tell DemoManager to check the queue and continue to next turn.
        EventManager.Instance.InvokeEvent(EventType.CheckQueue, null);
    }

    // A function used to calculate and push enemy actions to the queue.
    public void TakeEnemyAction()
    {
        display.SetActive(false);
        Debug.Log("Calculating enemy action...");
        BaseEnemy actingCharacter = (BaseEnemy)EventManager.Instance.EventData;
        
        // TODO: This AI is very simple. Should change to be more interesting.
        // Choose random party member and use Attack ability.
        BasePlayer target = party[UnityEngine.Random.Range(0, party.Count)];
        BaseAbility ability = actingCharacter.enemyClass.abilities[0];

        // Apply effects of ability and log the outcome.
        target.health -= ability.damage;
        Debug.Log(actingCharacter.name + " deals " + ability.damage + " damage to " + target.name + "!");

        // Tell DemoManager to check the queue and continue to next turn.
        EventManager.Instance.InvokeEvent(EventType.CheckQueue, null); 
    }

    // A function used to determine if any combatants are at 0 health; i.e., downed.
    public void CheckCombatantsHealth(ITargetable target)
    {
        if (target.health <= 0)
        {
            Debug.Log(target.name + " was downed in the gig!");
            RemoveCharacterFromCombat(target);
        }
    }

    // A function used to remove a downed character from combat.
    public void RemoveCharacterFromCombat(ITargetable character)
    {
        Tuple<BasePlayer, BaseEnemy> parsedCharacter = ParseTargetable(character);
        // Remove instance of character from appropriate list and ensure no turns in the queue belong to the downed character.
        if (parsedCharacter.Item1 != null)
        {
            party.Remove(parsedCharacter.Item1);
            // TODO: Fix this!
            foreach (PlayerTurn t in combatQueue.queue.ToArray())
            {
                if (t.actingCharacter == parsedCharacter.Item1)
                {
                    combatQueue.Remove(t);
                }
            }
        }
        if (parsedCharacter.Item2 != null)
        {
            enemies.Remove(parsedCharacter.Item2);
            // TODO: Fix this!
            foreach (EnemyTurn t in combatQueue.queue.ToArray())
            {
                if (t.actingCharacter == parsedCharacter.Item2)
                {
                    combatQueue.Remove(t);
                }
            }
        }
        // Update UI to no longer show character icon.
        stage.transform.Find(character.name + "Icon").gameObject.SetActive(false);
        // Update targetmenu to no longer show character as valid target.
        RemoveTargetButton(character.name);
        // Check for win/loss.
        CheckForWinLoss();
    }

    // A function used to determine a win/loss of combat.
    public void CheckForWinLoss() 
    {
        if (party.Count == 0)
        {
            Debug.Log("Player has lost!");
            EventManager.Instance.InvokeEvent(EventType.CombatLoss, null);
        }
        if (enemies.Count == 0)
        {
            Debug.Log("Player has won!");
            EventManager.Instance.InvokeEvent(EventType.CombatWin, null);
        }
    }

    // A helper function used to return a Tuple<BasePlayer, BaseEnemy> from an ITargetable object.
    // TODO: This is pretty hacky. Find a better solution.
    public Tuple<BasePlayer, BaseEnemy> ParseTargetable(ITargetable character)
    {
        Tuple<BasePlayer, BaseEnemy> output = new Tuple<BasePlayer, BaseEnemy>(null, null);
        foreach (BasePlayer p in party)
        {
            if (p.name == character.name)
            {
                output = new Tuple<BasePlayer, BaseEnemy>(p, null);
            }
        }
        foreach (BaseEnemy e in enemies)
        {
            if (e.name == character.name)
            {
                output = new Tuple<BasePlayer, BaseEnemy>(null, e);
            }
        }
        return output;
    }
}
