using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public BaseEnemy actingCharacter { get; set; }
    public void Execute()
    {
        EventManager.Instance.InvokeEvent(EventType.EnemyAI, actingCharacter);
        Debug.Log("It is currently " + actingCharacter.name + "'s turn.");
    }

    public EnemyTurn(BaseEnemy actingCharacter)
    {
        this.actingCharacter = actingCharacter;
    }
}
