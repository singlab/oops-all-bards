using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait
{
    public enum TraitTypes
    {
        NONE,
        SARDONIC,
        VENGEFUL
    }

    [SerializeField] private TraitTypes type;

    public TraitTypes Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

    public Trait(TraitTypes type)
    {
        this.type = type;
    }
}
