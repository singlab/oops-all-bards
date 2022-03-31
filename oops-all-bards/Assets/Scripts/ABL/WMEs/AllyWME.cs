using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyWME
{
    // General statistics
    public int id { get; set; }
    public float locationX { get; set; }
    public float locationY { get; set; }
    public float locationZ { get; set; }
    public bool inCombat { get; set; }

    // Combat statistics
    public int health { get; set; }
    public int flourish { get; set; }
    public int shield { get; set; }
    public int[] abilityIDs { get; set; }
    public int[] abilityCosts { get; set; }
    public string[] abilityTypes { get; set; }

    // CiF statistics
    public int[] partyIDs { get; set; }
    public int[] partyAffinities { get; set; }
    public int[] statusIDs { get; set; }
    public int[] traitIDs { get; set; }

    // We need a constructor of some sort. What are the best things to pass to this constructor to get
    // all the information we need? 
    
    // The BasePlayer class for each ally has most of the information we need.
    // The PartyManager class has current info on:
        // 1. Members of the party
        // 2. Whether or not the party is in combat

    // For the rest, maybe BasePlayer should be modified so that it contains info on:
        // 1. Affinities from this character to other characters -- a List<Tuple<int[characterID],int[affinityValue]>>
        // 2. Statuses for this character
        // 3. Traits for this character   
}
