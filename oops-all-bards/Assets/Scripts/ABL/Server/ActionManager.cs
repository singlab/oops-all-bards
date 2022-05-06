using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private static ActionManager _instance;
    public static ActionManager Instance => ActionManager._instance;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // A function used to parse an ABLResponse for further processing.
    // Codes: 1 -- Combat
    //        2 -- Noncombat
    public void ParseData(ABLResponse response) 
    {
        if (response.code == 1)
        {
            ManageCombatAction(response);
        } else
        {
            ManageNoncombatAction(response);
        }
    }

    public void ManageCombatAction(ABLResponse response)
    {
        if (response.msg == "Protect")
        {
            BasePlayer actingCharacter = PartyManager.Instance.FindPartyMemberById(response.data.actingCharacter);
            ITargetable target = PartyManager.Instance.FindPartyMemberById(response.data.targetCharacter);
            AllyAction action = new AllyAction(actingCharacter, target, AllyAction.ActionTypes.PROTECT);
            // Add this action to the queue with priority.
            CombatManager.Instance.combatQueue.PriorityPush(action);
        }
    }

    public void ManageNoncombatAction(ABLResponse response) 
    {
        Debug.Log("Received a noncombat action.");
    } 
}
