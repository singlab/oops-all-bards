using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAction : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public BasePlayer actingCharacter { get; set; }
    public ITargetable target { get; set; }
    public enum ActionTypes {
        PROTECT,
    }

    public ActionTypes actionType { get; set; }
    public void Execute()
    {
        Debug.Log("Executing " + actingCharacter.name + "'s " + actionType.ToString() + " action on " + target.name + ".");
        done = true;
    }

    public AllyAction(BasePlayer actingCharacter, ITargetable target, ActionTypes actionType)
    {
        this.actingCharacter = actingCharacter;
        this.target = target;
        this.actionType = actionType;
    }
}
