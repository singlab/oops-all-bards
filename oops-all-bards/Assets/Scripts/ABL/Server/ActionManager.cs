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
        ActionData data = JsonUtility.FromJson<ActionData>(response.data);
        if (response.code == 1)
        {
            ManageCombatAction(response, data);
        } else
        {
            ManageNoncombatAction(response, data);
        }
    }

    public ICombatQueueable ManageCombatAction(ABLResponse response, ActionData data)
    {
        if (response.msg == "Protect")
        {
            BasePlayer actingCharacter = PartyManager.Instance.FindPartyMemberById(data.actingCharacter);
            ITargetable target = PartyManager.Instance.FindPartyMemberById(data.targetCharacter);
            AllyAction action = new AllyAction(actingCharacter, target, AllyAction.ActionTypes.PROTECT);
            return action;
        }
        return null;
    }

    public void ManageNoncombatAction(ABLResponse response, ActionData data) 
    {
        Debug.Log("Received a noncombat action.");
    } 
}

// A class used to hold the characters IDs sent as part of the data field of an ABLResponse.
public class ActionData 
{
    public int actingCharacter;
    public int targetCharacter;
}
