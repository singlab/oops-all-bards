using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // A function used to initialize the combat queue.
    public void InitCombatQueue(List<BasePlayer> party, List<BaseEnemy> enemies)
    {
        // Clean up for new combat scenario.
        combatQueue = new CombatQueue();
        combatQueue.Clear();
        // Add standard functions for start, player input, enemy AI, and end.
        PushAndCreateCombatQueueable(new CombatStart());
        PushAndCreateCombatQueueable(new PlayerTurn(party[0]));
        PushAndCreateCombatQueueable(new EnemyTurn(enemies[0]));
        // Flag the DemoManager to begin checking queue.
        EventManager.Instance.InvokeEvent(EventType.CombatStart, null);
    }

    // A function used to push a CombatQueueable to the CombatQueue and create associated gameobject.
    public void PushAndCreateCombatQueueable(ICombatQueueable queueable)
    {
        // Push the queueable to the CombatQueue.
        combatQueue.Push(queueable);
        // Create gameobject and set container as parent.
        Instantiate(goQueueable, queueableContainer.transform.GetChild(0).transform);
    }

    public static CombatManager Instance
    {
        get 
        {
            if (_instance == null)
            {
                Debug.LogError("CombatManager is NULL.");
            }
            return _instance;
        }
    }
}
