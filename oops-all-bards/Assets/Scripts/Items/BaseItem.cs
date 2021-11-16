using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem
{
    public string name { get; set; }
    public string description { get; set; }
    public ItemTypes type { get; set; }
    public int value { get; set; }

    public enum ItemTypes
    {
        DUDS,
        INSTRUMENTS,
        POTABLES
    }
}
