using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public BasePlayer player;
    public PlayerTurn newTurn;

    public void Execute()
    {
        Debug.Log("back button is doing something");

        CombatManager.Instance.combatQueue.PriorityPush(new PlayerTurn(player));
        EventManager.Instance.InvokeEvent(EventType.CheckQueue, null);
        //Command graveyard

        //CombatManager.Instance.InitCombatQueue(CombatManager.Instance.party, CombatManager.Instance.enemies);
        // CombatManager.Instance.ClearCombatMenu();
        //CombatManager.Instance.combatQueue.Remove(newTurn);
        //CombatManager.Instance.RenderInputMenu(player);
        //EventManager.Instance.InvokeEvent(EventType.CheckQueue, null);

        // CombatManager.Instance.combatQueue = new CombatQueue();
        //destroy turn
        //create new turn


    }
}
