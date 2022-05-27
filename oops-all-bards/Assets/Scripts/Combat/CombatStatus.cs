using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CombatStatus
{
    [SerializeField] private CombatStatusTypes type;

    public CombatStatusTypes Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

    public enum CombatStatusTypes 
    {
        NONE,
        PROTECTED,
        PROTECTING,
        STRENGTHENED,
        BLINDED,
        CONFIDENT,
        BLOODIED
    }

    public CombatStatus(CombatStatusTypes type)
    {
        this.type = type;
    }
}
