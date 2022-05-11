using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private StatTypes type;
    [SerializeField] private int baseValue;
    [SerializeField] private int modifiedValue;

    public enum StatTypes
    {
        FLOURISH,
        ORATORY,
        RHAPSODY,
        TEMPO,
        ELAN
    }

    public BaseStat(string name, string desc, StatTypes type, int baseValue, int modifiedValue)
    {
        this.name = name;
        this.description = desc;
        this.type = type;
        this.baseValue = baseValue;
        this.modifiedValue = modifiedValue;
    }

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public string Description
    {
        get { return this.description; }
        set { this.description = value; }
    }

    public StatTypes Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

    public int BaseValue
    {
        get { return this.baseValue; }
        set { this.baseValue = value; }
    }

    public int ModifiedValue
    {
        get { return this.modifiedValue; }
        set { this.modifiedValue = value; }
    }
}
