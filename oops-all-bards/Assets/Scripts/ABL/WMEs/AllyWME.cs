using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyWME
{
    // General statistics
    public int id { get; set; }
    // public float locationX { get; set; }
    // public float locationY { get; set; }
    // public float locationZ { get; set; }
    public bool inCombat { get; set; }

    // Combat statistics
    public int health { get; set; }
    // public int flourish { get; set; }
    public int shield { get; set; }
    public int[] abilityIDs { get; set; }
    public int[] abilityCosts { get; set; }
    public string[] abilityTypes { get; set; }
    public bool ownsTurn { get; set; }

    // CiF statistics
    public int[] partyIDs { get; set; }
    public int[] partyAffinities { get; set; }
    public int[] statusIDs { get; set; }
    public int[] traitIDs { get; set; }

    public AllyWME(BasePlayer player)
    {
        this.id = player.id;
        // TODO: When we have 3D movement of player/allies, this will become important.
        // this.locationX = player.locationX;
        // this.locationY = player.locationY;
        // this.locationZ = player.locationZ;
        this.inCombat = PartyManager.Instance.inCombat;
        this.health = player.health;
        // TODO: Implement flourish plz
        // this.flourish = player.flourish;
        this.shield = player.shield;
        // Handle ability data
        this.abilityIDs = new int[player.playerClass.abilities.Count];
        this.abilityCosts = new int[player.playerClass.abilities.Count];
        this.abilityTypes = new string[player.playerClass.abilities.Count];
        for (int i = 0; i < player.playerClass.abilities.Count; i++)
        {
            abilityIDs[i] = player.playerClass.abilities[i].id;
            abilityCosts[i] = player.playerClass.abilities[i].cost;
            abilityTypes[i] = player.playerClass.abilities[i].combatType.ToString();
        }
        this.ownsTurn = false;
        // Handle CiF data
        this.partyIDs = new int[PartyManager.Instance.currentParty.Count];
        this.partyAffinities = new int[PartyManager.Instance.currentParty.Count];
        this.statusIDs = new int[player.cifData.statuses.Count];
        this.traitIDs = new int[player.cifData.traits.Count];
        for (int i = 0; i < PartyManager.Instance.currentParty.Count; i++)
        {
            partyIDs[i] = PartyManager.Instance.currentParty[i].id;
            partyAffinities[i] = PartyManager.Instance.currentParty[i].cifData.GetAffinityByID(partyIDs[i]);
        }
        for (int i = 0; i < player.cifData.statuses.Count; i++)
        {
            statusIDs[i] = player.cifData.statuses[i].id;
        }
        for (int i = 0; i < player.cifData.traits.Count; i++)
        {
            traitIDs[i] = player.cifData.traits[i].id;
        }
    }   
}
