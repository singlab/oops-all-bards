using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class BaseClass 
{

	private string name { get; set; }
	private string description { get; set; }
	private ClassTypes type { get; set; }
	private List<BaseStat> stats { get; set; } = new List<BaseStat>();
	private List<BaseAbility> abilities { get; set; } = new List<BaseAbility>();

	public enum ClassTypes
	{
		MUMMER,
		MUSICIAN,
		SKALD,
		SIREN
	}
}
