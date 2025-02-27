using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectAction : IABLAction
{
    public void Execute(ActionData data)
    {
        BasePlayer actingCharacter = PartyManager.Instance.FindPartyMemberById(data.actingCharacter);
        ITargetable target = PartyManager.Instance.FindPartyMemberById(data.targetCharacter);
        AllyAction action = new AllyAction(actingCharacter, target, AllyAction.ActionTypes.PROTECT);
        CombatManager.Instance.combatQueue.PriorityPush(action);
    }
}
