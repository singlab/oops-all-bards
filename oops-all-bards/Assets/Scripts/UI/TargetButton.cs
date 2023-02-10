using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public string target;
    public void Execute()
    {
        CombatManager.Instance.target = target;
        if (target == null)
        {
            Debug.Log("eyo target missing");
        }
        //CombatManager.Instance.ClearCombatMenu();
        CombatManager.Instance.UI.GetComponent<CombatUI>().ClearCombatMenu();
       
    }
}
