using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    private static CombatManager _instance;
    // A reference to the stage.
    GameObject stage;
    // A reference to the display area.
    GameObject display;
    // A reference to the gameobject queue.
    GameObject queueableContainer;
    // A reference to the queueable gameobject prefab.
    public GameObject goQueueable;
    // A reference to the combat queue.
    public CombatQueue combatQueue;
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

        stage.SetActive(false);
        display.SetActive(false);
        queueableContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        foreach (Transform child in display.transform)
        {
            if (child.gameObject.name != activeMenu.name)
            {
                child.gameObject.SetActive(false);
            }
        }
        // Fill in the name, health, and ability names of the acting character.
        GameObject nameplate = GameObject.Find("Name");
        nameplate.GetComponent<Text>().text = actingCharacter.name;
        GameObject health = GameObject.Find("Health");
        health.GetComponent<Text>().text += actingCharacter.health.ToString();
        // TODO: This is pretty hacky, should probably go by number of abilities available to player and then make button prefabs/place them in menu.
        for (int i = 1; i < 4; i++)
        {
            GameObject abilityButton = GameObject.Find("Ability" + i + "Text");
            abilityButton.GetComponent<Text>().text = actingCharacter.playerClass.abilities[i-1].name;
        }
    }

    // A function used to initialize the combat queue.
    public void InitCombatQueue(List<BasePlayer> party, List<BaseEnemy> enemies)
    {
        // Clean up for new combat scenario.
        combatQueue = new CombatQueue();
        combatQueue.Clear();
        // Add standard functions for start, player input, enemy AI, and end.
        PushAndCreateCombatQueueable(new CombatStart());
        foreach (BasePlayer p in party)
        {
            PushAndCreateCombatQueueable(new PlayerTurn(p));
        }
        foreach (BaseEnemy e in enemies)
        {
            PushAndCreateCombatQueueable(new EnemyTurn(e));
        }
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
}
