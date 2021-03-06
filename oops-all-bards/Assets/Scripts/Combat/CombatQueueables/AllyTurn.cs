using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyTurn : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public BasePlayer actingCharacter { get; set; }
    public void Execute()
    {
        Debug.Log("It is currently " + actingCharacter.Name + "'s turn.");
        actingCharacter.OwnsTurn = true;
        TCPTestClient.Instance.RefreshWMEs();
        EventManager.Instance.InvokeEvent(EventType.AllyAI, actingCharacter);
    }

    public AllyTurn(BasePlayer actingCharacter)
    {
        this.actingCharacter = actingCharacter;
    }
}
