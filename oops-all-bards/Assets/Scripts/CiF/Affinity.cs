using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Affinity
{
    [SerializeField] private int characterID;
    [SerializeField] private int value;

    public Affinity(int characterID, int value)
    {
        this.characterID = characterID;
        this.value = value;
    }

    public void ModifyAffinity(int value)
    {
        this.value = value;
    }

    public int CharacterID
    {
        get { return this.characterID; }
        set { this.characterID = value; }
    }

    public int Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
}
