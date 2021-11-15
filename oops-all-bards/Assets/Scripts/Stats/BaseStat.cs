using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    private string name { get; set; }
    private string description { get; set; }
    private StatTypes type { get; set; }
    private int baseValue { get; set; }
    private int modifiedValue { get; set; }

    public enum StatTypes
    {
        FLOURISH,
        ORATORY,
        RHETORIC,
        TEMPO,
        ELAN
    }
}
