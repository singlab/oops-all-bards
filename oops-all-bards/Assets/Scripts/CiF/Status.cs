using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    public string name { get; set; }
    public int id { get; set; }

    public Status(string name, int id)
    {
        this.name = name;
        this.id = id;
    }
}
