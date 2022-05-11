using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public BaseAbility ability { get; set; }
    public BasePlayer actingCharacter { get; set; }
    public ITargetable target { get; set; }
    public void Execute()
    {
        Debug.Log("Executing " + actingCharacter.name + "'s " + ability.Name + " action on " + target.name + ".");
        CombatManager.Instance.ResolvePlayerAction(this);
        actingCharacter.ownsTurn = false;
        done = true;
    }

    public PlayerAction(BaseAbility ability, BasePlayer actingCharacter, ITargetable target)
    {
        this.ability = ability;
        this.actingCharacter = actingCharacter;
        this.target = target;
    }
}
