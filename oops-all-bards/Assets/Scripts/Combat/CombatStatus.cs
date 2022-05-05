using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatStatus
{
    public StatusTypes type { get; set; }

    public enum StatusTypes 
    {
        PROTECTED,
        PROTECTING
    }

    public CombatStatus(StatusTypes type)
    {
        this.type = type;
    }
}
