using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private ItemTypes type;
    [SerializeField] private int value;

    public enum ItemTypes
    {
        DUDS,
        INSTRUMENTS,
        POTABLES
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

    public ItemTypes Type
    {
        get { return this.type; }
        set { this.type = value; }
    }

    public int Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
}
