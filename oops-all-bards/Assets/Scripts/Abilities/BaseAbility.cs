using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAbility
{
    public string name { get; set; }
	public string description { get; set; }
	public AbilityTypes type { get; set; }
	public int damage { get; set; }
	public int cost { get; set; }
	public int currentRank { get; set; }
	public int maxRank { get; set; }

	public enum AbilityTypes
	{
		COMBAT,
		PASSIVE
	}

	public BaseAbility(string name, string desc, AbilityTypes type, int damage, int cost, int currentRank, int maxRank)
	{
		this.name = name;
		this.description = desc;
		this.type = type;
		this.damage = damage;
		this.cost = cost;
		this.currentRank = currentRank;
		this.maxRank = maxRank;
	}
}
