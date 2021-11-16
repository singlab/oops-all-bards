using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemy
{
    public string name { get; set; }
    public BaseClass enemyClass { get; set; }

    public BaseEnemy(string name, BaseClass enemyClass)
    {
        this.name = name;
        this.enemyClass = enemyClass;
    }
}
