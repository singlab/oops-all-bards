using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public BasePlayer actingCharacter { get; set; }
    public void Execute()
    {
        // Flag to begin waiting for player input.
        EventManager.Instance.InvokeEvent(EventType.AwaitPlayerInput, actingCharacter);
        CombatManager.Instance.RenderInputMenu(actingCharacter);
        Debug.Log("It is currently " + actingCharacter.name + "'s turn.");
        done = true;
    }

    public PlayerTurn(BasePlayer actingCharacter)
    {
        this.actingCharacter = actingCharacter;
    }
}
