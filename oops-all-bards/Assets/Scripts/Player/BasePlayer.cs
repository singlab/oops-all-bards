using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePlayer : ITargetable
{
    public override string name { get; set; }
    public int id { get; set; }
    public override int health { get; set; }
    public override int flourish { get; set; }
    public override int shield { get; set; }
    public BaseClass playerClass { get; set; }
    public List<BaseStat> playerStats { get; set; } = new List<BaseStat>();
    public int fame { get; set; }
	public int gold { get; set; }
    public List<BaseItem> equipment { get; set; } = new List<BaseItem>();
	public List<BaseItem> inventory { get; set; } = new List<BaseItem>();
    public CiFData cifData { get; set; }

    public BasePlayer(string name, int id, int health, int flourish, int shield, BaseClass playerClass, List<BaseStat> playerStats, int fame, int gold, List<BaseItem> equipment, List<BaseItem> inventory)
    {
        this.name = name;
        this.id = id;
        this.health = health;
        this.flourish = flourish;
        this.shield = shield;
        this.playerClass = playerClass;
        this.playerStats = playerStats;
        this.fame = fame;
        this.gold = gold;
        this.equipment = equipment;
        this.inventory = inventory;
        this.cifData = new CiFData();
    }
}
