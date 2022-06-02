using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStart : MonoBehaviour, ICombatQueueable
{
    public bool done { get; set; }
    public void Execute() 
    {
        // UI idea: Opening curtains. This should execute here.
        Debug.Log("The gig begins!");
        if (CombatManager.Instance.rounds > 1)
        {
            EventManager.Instance.InvokeEvent(EventType.CheckQueue, null);
        }
        done = true;
    }
}
