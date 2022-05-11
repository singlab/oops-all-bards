using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BaseClass 
{

	[SerializeField] private string name;
	[SerializeField] private string description;
	[SerializeField] private ClassTypes type;
	[SerializeField] private List<BaseStat> stats = new List<BaseStat>();
	[SerializeField] private List<BaseAbility> abilities = new List<BaseAbility>();

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

	public string Name 
	{
		get { return this.name; }
		set { this.name = value; }
	}

	public string Description 
	{
		get { return this.description; }
		set { this.description = value; }
	}

	public ClassTypes Type 
	{
		get { return this.type; }
		set { this.type = value; }
	}

	public List<BaseStat> Stats
	{
		get { return this.stats; }
		set { this.stats = value; }
	}

	public List<BaseAbility> Abilities
	{
		get { return this.abilities; }
		set { this.abilities = value; }
	}
}

[System.Serializable]
public class BaseClasses
{
	[SerializeField] public List<BaseClass> baseClasses = new List<BaseClass>();
}
