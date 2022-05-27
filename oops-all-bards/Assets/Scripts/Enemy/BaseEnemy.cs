using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy : ITargetable
{
    private string name;
    private int health;
    private int flourish;
    private int shield;
    private BaseClass enemyClass;
    private bool ownsTurn;
    private List<CombatStatus> combatStatuses = new List<CombatStatus>();

    public BaseEnemy(string name, int health, int flourish, int shield, BaseClass enemyClass)
    {
        this.name = name;
        this.health = health;
        this.flourish = flourish;
        this.shield = shield;
        this.enemyClass = enemyClass;
    }

    public override string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public override int Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    public override int Flourish
    {
        get { return this.flourish; }
        set { this.flourish = value; }
    }

    public override int Shield
    {
        get { return this.shield; }
        set { this.shield = value; }
    }

    public BaseClass EnemyClass
    {
        get { return this.enemyClass; }
        set { this.enemyClass = value; }
    }

    public bool OwnsTurn
    {
        get { return this.ownsTurn; }
        set { this.ownsTurn = value; }
    }

    public override List<CombatStatus> CombatStatuses
    {
        get { return this.combatStatuses; }
        set { this.combatStatuses = value; }
    }

    public override void RemoveCombatStatus(CombatStatus.CombatStatusTypes type)
    {
        foreach (CombatStatus cs in this.CombatStatuses.ToArray())
        {
            if (cs.Type == type)
            {
                this.CombatStatuses.Remove(cs);
            }
        }
    }

    public override bool HasCombatStatusType(CombatStatus.CombatStatusTypes type)
    {
        foreach (CombatStatus cs in this.CombatStatuses)
        {
            if (cs.Type == type)
            {
                return true;
            }
        }
        return false;
    }
}
