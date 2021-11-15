using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAbility
{
    private string name { get; set; }
	private string description { get; set; }
	private AbilityTypes type { get; set; }
	private int damage { get; set; }
	private int cost { get; set; }
	private int currentRank { get; set; }
	private int maxRank { get; set; }

	public enum AbilityTypes
	{
		COMBAT,
		PASSIVE
	}
}
