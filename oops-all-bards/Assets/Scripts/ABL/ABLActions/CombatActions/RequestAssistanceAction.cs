using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestAssistanceAction : IABLAction
{
    public void Execute(ActionData data)
    {
        BasePlayer actingCharacter = PartyManager.Instance.FindPartyMemberById(data.actingCharacter);
		actingCharacter.CiFData.AddStatus(new Status(Status.StatusTypes.REQUIRES_ASSISTANCE));
        // Dialogue trigger?
    }
}
