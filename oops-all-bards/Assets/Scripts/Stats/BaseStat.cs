using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public string name { get; set; }
    public string description { get; set; }
    public StatTypes type { get; set; }
    public int baseValue { get; set; }
    public int modifiedValue { get; set; }

    public enum StatTypes
    {
        FLOURISH,
        ORATORY,
        RHETORIC,
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
}
