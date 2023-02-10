using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public string target;
    public void Execute()
    {
        CombatManager.Instance.target = target;
        CombatManager.Instance.UI.GetComponent<CombatUI>().ClearCombatMenu();
       
    }
}
