using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllyWME
{
    // General statistics
    [SerializeField] public int id;
    // [SerializeField] public float locationX;
    // [SerializeField] public float locationY;
    // [SerializeField] public float locationZ;
    [SerializeField] public bool inCombat;

    // Combat statistics
    [SerializeField] public int health;
    [SerializeField] public int flourish;
    [SerializeField] public int shield;
    [SerializeField] public int[] abilityIDs;
    [SerializeField] public int[] abilityCosts;
    [SerializeField] public string[] abilityTypes;
    [SerializeField] public bool ownsTurn;

    // CiF statistics
    [SerializeField] public int[] partyIDs;
    [SerializeField] public int[] partyAffinities;
    [SerializeField] public int[] statusIDs;
    [SerializeField] public int[] traitIDs;

    public AllyWME(BasePlayer player)
    {
        this.id = player.id;
        // TODO: When we have 3D movement of player/allies, this will become important.
        // this.locationX = player.locationX;
        // this.locationY = player.locationY;
        // this.locationZ = player.locationZ;
        this.inCombat = PartyManager.Instance.inCombat;
        this.health = player.health;
        this.flourish = player.flourish;
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
