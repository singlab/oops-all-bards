using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePlayer
{
    public string name { get; set; }
    public int health { get; set; }
    public BaseClass playerClass { get; set; }
    public List<BaseStat> playerStats { get; set; } = new List<BaseStat>();
    public int fame { get; set; }
	public int gold { get; set; }
    public List<BaseItem> equipment { get; set; } = new List<BaseItem>();
	public List<BaseItem> inventory { get; set; } = new List<BaseItem>();

    public BasePlayer(string name, int health, BaseClass playerClass, List<BaseStat> playerStats, int fame, int gold, List<BaseItem> equipment, List<BaseItem> inventory)
    {
        this.name = name;
        this.health = health;
        this.playerClass = playerClass;
        this.playerStats = playerStats;
        this.fame = fame;
        this.gold = gold;
        this.equipment = equipment;
        this.inventory = inventory;
    }
}
