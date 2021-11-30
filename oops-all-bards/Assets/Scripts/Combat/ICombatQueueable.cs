using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface for a queueable combat command. Fed to the CombatQueue
// and interpreted by the CombatManager.
public interface ICombatQueueable
{
    bool done { get; set; }
    void Execute();
}
