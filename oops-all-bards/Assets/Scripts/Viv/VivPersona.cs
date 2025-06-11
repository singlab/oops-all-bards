using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewVivPersona", menuName = "Viv/Viv Persona")]
public class VivPersona : ScriptableObject
{
    public List<BehaviorPriority> behaviorPriorities = new List<BehaviorPriority>();

    // Helper method to get the priority for a given behavior name
    public int GetPriorityFor(string behaviorName)
    {
        var foundPriority = behaviorPriorities.FirstOrDefault(p => p.behaviorName == behaviorName);
        if (foundPriority != null)
        {
            return foundPriority.priority;
        }

        // Return a high number (low priority) if no specific priority is defined
        return 99;
    }
}

[System.Serializable]
public class BehaviorPriority
{
    public string behaviorName;
    [Tooltip("Lower number = higher priority")]
    public int priority;
}