using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatStatus
{
    [SerializeField] private StatusTypes type;

    public StatusTypes Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

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
