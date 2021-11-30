using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class that manages the features of the combat demo.
public class DemoManager : MonoBehaviour
{
    // A reference to the player party.
    List<BasePlayer> party = new List<BasePlayer>();
    // A reference to the enemies.
    List<BaseEnemy> enemies = new List<BaseEnemy>();
    // A reference to the stage.
    GameObject stage;
    // A reference to the display area.
    GameObject display;
    // A reference to the gameobject queue.
    GameObject queueableContainer;
    // A reference to the queueable gameobject prefab.
    public GameObject goQueueable;
    // A reference to the combat queue.
    CombatQueue combatQueue;

    // Start is called before the first frame update
    void Start()
    {
        stage = GameObject.Find("Stage");
        display = GameObject.Find("Display");
        queueableContainer = GameObject.Find("CombatQueue");

        stage.SetActive(false);
        display.SetActive(false);
        queueableContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // A function that initializes the demo with all actors and starting values.
    public void InitDemo()
    {
        // Init player and ally.
        BaseClass playerClass = CreatePlayerClass(BaseClass.ClassTypes.SKALD);
        BasePlayer player = new BasePlayer("Player", playerClass, playerClass.stats, 0, 0, null, null);
        BasePlayer ally = new BasePlayer("Ally", playerClass, playerClass.stats, 0, 0, null, null);
        party.Add(player);
        party.Add(ally);

        // Init enemies.
        BaseEnemy enemy = new BaseEnemy("Enemy 1", playerClass);
        enemies.Add(enemy);
        enemy = new BaseEnemy("Enemy 2", playerClass);
        enemies.Add(enemy);

        // Render the UI.
        RenderUI();

        // Initialize the combat queue.
        InitCombatQueue();
    }

    // A function that returns a class based on class type. Only for demo use.
    private BaseClass CreatePlayerClass(BaseClass.ClassTypes type)
    {
        BaseClass playerClass = null;
        if (type == BaseClass.ClassTypes.SKALD)
        {
            playerClass = new BaseClass("Skald", "A warrior poet.", BaseClass.ClassTypes.SKALD, CreateClassStats(BaseClass.ClassTypes.SKALD), CreateClassAbilities(BaseClass.ClassTypes.SKALD));       
        }

        return playerClass == null ? null : playerClass;
    }

    // A function that returns a list of stats based on class type. Only for demo use.
    private List<BaseStat> CreateClassStats(BaseClass.ClassTypes type)
    {
        List<BaseStat> stats = new List<BaseStat>();
        if (type == BaseClass.ClassTypes.SKALD)
        {
            BaseStat stat = new BaseStat("Flourish", "A measure of vigorous style.", BaseStat.StatTypes.FLOURISH, 5, 0);
            stats.Add(stat);
            stat = new BaseStat("Oratory", "A measure of refined speech.", BaseStat.StatTypes.ORATORY, 3, 0);
            stats.Add(stat);
            stat = new BaseStat("Rhetoric", "A measure of purposeful persuasion.", BaseStat.StatTypes.RHETORIC, 4, 0);
            stats.Add(stat);
            stat = new BaseStat("Tempo", "A measure of quickened pace.", BaseStat.StatTypes.TEMPO, 6, 0);
            stats.Add(stat);
            stat = new BaseStat("Elan", "A measure of great gusto.", BaseStat.StatTypes.ELAN, 4, 0);
        }

        return stats;
    }

    // A function that returns a list of abilities based on class type. Only for demo use.
    private List<BaseAbility> CreateClassAbilities(BaseClass.ClassTypes type)
    {
        List<BaseAbility> abilities = new List<BaseAbility>();
        if (type == BaseClass.ClassTypes.SKALD)
        {
            BaseAbility ability = new BaseAbility("Attack", "A basic attack.", BaseAbility.AbilityTypes.COMBAT, 3, 0, 1, 5);
            abilities.Add(ability);
            ability = new BaseAbility("Defend", "Take defensive precautions.", BaseAbility.AbilityTypes.COMBAT, 3, 0, 1, 5);
            abilities.Add(ability);
            ability = new BaseAbility("Shrug It Off", "Heal wounds.", BaseAbility.AbilityTypes.COMBAT, 3, 3, 1, 5);
            abilities.Add(ability);
        }

        return abilities;
    }

    // A function used to debug the player object.
    private void DebugPlayer(BasePlayer player)
    {
        Debug.Log(player.name);
        Debug.Log("FAME: " + player.fame);
        Debug.Log("GOLD: " + player.gold);
        Debug.Log(player.playerClass.name);
        foreach (BaseStat stat in player.playerStats)
        {
            Debug.Log(stat.name + " " + stat.baseValue);
        }
        foreach (BaseAbility ability in player.playerClass.abilities)
        {
            Debug.Log(ability.name + " " + ability.damage + " " + ability.cost);
        }
    }

    // A function used to render all UI elements for the demo.
    private void RenderUI()
    {
        // Disable the init button.
        GameObject.Find("InitButton").SetActive(false);

        // Render the placeholder stage and queueable container.
        stage.SetActive(true);
        queueableContainer.SetActive(true);
    }

    // A function used to initialize the combat queue.
    private void InitCombatQueue()
    {
        // Clean up for new combat scenario.
        combatQueue = new CombatQueue();
        combatQueue.Clear();
        // Add standard functions for start, player input, enemy AI, and end.
        PushAndCreateCombatQueueable(new CombatStart());
        PushAndCreateCombatQueueable(new PlayerTurn(party[0]));
        PushAndCreateCombatQueueable(new EnemyTurn(enemies[0]));
    }

    // A function used to push a CombatQueueable to the CombatQueue and create associated gameobject.
    private void PushAndCreateCombatQueueable(ICombatQueueable queueable)
    {
        // Push the queueable to the CombatQueue.
        combatQueue.Push(queueable);
        // Create gameobject and set container as parent.
        Instantiate(goQueueable, queueableContainer.transform.GetChild(0).transform);
    }
}
