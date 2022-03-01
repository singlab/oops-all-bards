using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public BaseAbility ability;
    public BasePlayer actingCharacter;

    public void Execute()
    {
        CombatManager.Instance.ClearCombatMenu();
        CombatManager.Instance.RenderTargetButtons();
    }
}
