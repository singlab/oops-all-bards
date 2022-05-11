using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BaseClass 
{

	public string name { get; set; }
	public string description { get; set; }
	public ClassTypes type { get; set; }
	public List<BaseStat> stats { get; set; } = new List<BaseStat>();
	public List<BaseAbility> abilities { get; set; } = new List<BaseAbility>();

	public enum ClassTypes
	{
		MUMMER,
		MUSICIAN,
		SKALD,
		SIREN
	}

	public BaseClass(string name, string desc, ClassTypes type, List<BaseStat> stats, List<BaseAbility> abilities) 
	{
		this.name = name;
		this.description = desc;
		this.type = type;
		this.stats = stats;
		this.abilities = abilities;
	}
}

[System.Serializable]
public class BaseClasses
{
	public List<BaseClass> baseClasses = new List<BaseClass>();
}
