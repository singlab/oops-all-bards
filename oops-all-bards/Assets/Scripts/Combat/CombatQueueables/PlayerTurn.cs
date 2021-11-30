using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public BasePlayer actingCharacter { get; set; }
    public void Execute()
    {
        Debug.Log("Waiting for player input...");
        Debug.Log("It is currently " + actingCharacter.name + "'s turn.");
    }

    public PlayerTurn(BasePlayer actingCharacter)
    {
        this.actingCharacter = actingCharacter;
    }
}
