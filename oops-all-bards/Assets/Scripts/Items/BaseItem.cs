using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem
{
    private string name { get; set; }
    private string description { get; set; }
    private ItemTypes type { get; set; }
    private int value { get; set; }

    public enum ItemTypes
    {
        DUDS,
        INSTRUMENTS,
        POTABLES
    }
}
