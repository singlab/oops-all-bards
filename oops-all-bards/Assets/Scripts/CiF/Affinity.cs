using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Affinity
{
    public int characterID { get; set; }
    public int value { get; set; }

    public Affinity(int characterID, int value)
    {
        this.characterID = characterID;
        this.value = value;
    }

    public void ModifyAffinity(int value)
    {
        this.value = value;
    }
}
