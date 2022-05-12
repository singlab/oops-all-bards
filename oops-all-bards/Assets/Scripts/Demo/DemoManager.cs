using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// A class that manages the features of the combat demo.
public class DemoManager : MonoBehaviour
{
    // A reference to the player party.
    List<BasePlayer> party = new List<BasePlayer>();
    // A reference to the enemies.
    List<BaseEnemy> enemies = new List<BaseEnemy>();

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to events that the demo manager should be aware of.
        SubscribeToEvents();
        // Setup party and enemies for demo.
        GatherParty();
        // Have CombatManager init combat with preselected party/enemies above.
        CombatManager.Instance.InitCombatQueue(party, enemies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // A function that initializes the demo with all actors and starting values.
    public void GatherParty()
    {
        // Init player and ally.
        BaseClass playerClass = CreatePlayerClass(BaseClass.ClassTypes.SKALD);
        BasePlayer player = new BasePlayer("Player", 0, 20, 10, 0, playerClass, playerClass.Stats, 0, 0, null, null);
        BasePlayer ally = new BasePlayer("Ally", 1, 20, 10, 0, playerClass, playerClass.Stats, 0, 0, null, null);
        party.Add(player);
        party.Add(ally);

        // Init enemies.
        BaseEnemy enemy = new BaseEnemy("Enemy1", 10, 5, 0, playerClass);
        enemies.Add(enemy);
        enemy = new BaseEnemy("Enemy2", 10, 5, 0, playerClass);
        enemies.Add(enemy);
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
            stat = new BaseStat("Rhapsody", "A measure of enchanting persuasion.", BaseStat.StatTypes.RHAPSODY, 4, 0);
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
            BaseAbility ability = new BaseAbility("Attack", 0, "A basic attack.", BaseAbility.AbilityTypes.COMBAT, BaseAbility.CombatAbilityTypes.ATTACK, 3, 0, 1, 5);
            abilities.Add(ability);
            ability = new BaseAbility("Defend", 1, "Take defensive precautions.", BaseAbility.AbilityTypes.COMBAT, BaseAbility.CombatAbilityTypes.DEFEND, 3, 0, 1, 5);
            abilities.Add(ability);
            ability = new BaseAbility("Shrug It Off", 2, "Heal wounds.", BaseAbility.AbilityTypes.COMBAT, BaseAbility.CombatAbilityTypes.HEAL, 3, 3, 1, 5);
            abilities.Add(ability);
        }

        return abilities;
    }

    // A function used to debug the player object.
    private void DebugPlayer(BasePlayer player)
    {
        Debug.Log(player.Name);
        Debug.Log("FAME: " + player.Fame);
        Debug.Log("GOLD: " + player.Gold);
        Debug.Log(player.PlayerClass.Name);
        foreach (BaseStat stat in player.PlayerStats)
        {
            Debug.Log(stat.Name + " " + stat.BaseValue);
        }
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
        SceneManager.LoadScene("CombatWin");
        PartyManager.Instance.ToggleInCombat(false);
    }

    public void CombatLoss()
    {
        SceneManager.LoadScene("CombatLoss");
        PartyManager.Instance.ToggleInCombat(false);
    }
}
