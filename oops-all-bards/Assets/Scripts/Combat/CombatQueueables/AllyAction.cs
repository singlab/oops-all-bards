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
        ResolveAction();
        done = true;
        // Tell DemoManager to check the queue and continue to next turn.
        EventManager.Instance.InvokeEvent(EventType.CheckQueue, null);
    }

    public AllyAction(BasePlayer actingCharacter, ITargetable target, ActionTypes actionType)
    {
        this.actingCharacter = actingCharacter;
        this.target = target;
        this.actionType = actionType;
    }

    private void ResolveAction()
    {
        // TODO: Rethink how we handle the different action types. There will be too many ifs.
        if (this.actionType == ActionTypes.PROTECT)
        {
            BasePlayer targetCharacter = ParseTargetable(target);
            Debug.Log("Through the power of friendship, " + actingCharacter.Name + " is protecting " + target.Name + "!");
            actingCharacter.CombatStatuses.Add(new CombatStatus(CombatStatus.StatusTypes.PROTECTING));
            targetCharacter.CombatStatuses.Add(new CombatStatus(CombatStatus.StatusTypes.PROTECTED));
            Debug.Log(actingCharacter.Name + " has " + actingCharacter.CombatStatuses.Count + " combat statuses.");
            Debug.Log(targetCharacter.Name + " has " + targetCharacter.CombatStatuses.Count + " combat statuses.");
        }
    }

    private BasePlayer ParseTargetable(ITargetable target)
    {
        foreach (BasePlayer p in CombatManager.Instance.party)
        {
            if (p.Name == target.Name)
            {
                return p;
            }
        }
        return null;
    }
}
