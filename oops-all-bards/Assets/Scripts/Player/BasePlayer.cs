using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePlayer
{
    private string name { get; set; }
    private BaseClass playerClass { get; set; }
    private List<BaseStat> playerStats { get; set; } = new List<BaseStat>();
    private int fame { get; set; }
	private int gold { get; set; }
    private List<BaseItem> equipment { get; set; } = new List<BaseItem>();
	private List<BaseItem> inventory { get; set; } = new List<BaseItem>();
}
