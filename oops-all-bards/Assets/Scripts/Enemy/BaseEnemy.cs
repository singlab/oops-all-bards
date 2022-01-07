using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy : ITargetable
{
    public override string name { get; set; }
    public override int health { get; set; }
    public override int shield { get; set; }
    public BaseClass enemyClass { get; set; }

    public BaseEnemy(string name, int health, int shield, BaseClass enemyClass)
    {
        this.name = name;
        this.health = health;
        this.shield = shield;
        this.enemyClass = enemyClass;
    }
}
