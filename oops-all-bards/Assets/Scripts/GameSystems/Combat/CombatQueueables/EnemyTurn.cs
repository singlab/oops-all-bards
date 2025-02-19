using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public BaseEnemy actingCharacter { get; set; }
    public void Execute()
    {
        Debug.Log("It is currently " + actingCharacter.Name + "'s turn.");
        actingCharacter.OwnsTurn = true;
        EventManager.Instance.InvokeEvent(EventType.EnemyAI, actingCharacter);
    }

    public EnemyTurn(BaseEnemy actingCharacter)
    {
        this.actingCharacter = actingCharacter;
    }
}
