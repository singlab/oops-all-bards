using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePlayer : ITargetable
{
    public override string name { get; set; }
    // TODO: Add ID field
    public override int health { get; set; }
    public override int shield { get; set; }
    public BaseClass playerClass { get; set; }
    public List<BaseStat> playerStats { get; set; } = new List<BaseStat>();
    public int fame { get; set; }
	public int gold { get; set; }
    public List<BaseItem> equipment { get; set; } = new List<BaseItem>();
	public List<BaseItem> inventory { get; set; } = new List<BaseItem>();
    // TODO: Add affinities
    // TODO: Add statuses
    // TODO: Add traits

    public BasePlayer(string name, int health, int shield, BaseClass playerClass, List<BaseStat> playerStats, int fame, int gold, List<BaseItem> equipment, List<BaseItem> inventory)
    {
        this.name = name;
        this.health = health;
        this.shield = shield;
        this.playerClass = playerClass;
        this.playerStats = playerStats;
        this.fame = fame;
        this.gold = gold;
        this.equipment = equipment;
        this.inventory = inventory;
    }
}
