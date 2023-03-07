using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private JSONReader jsonReader;

    private static EnemyFactory _instance;
    public static EnemyFactory Instance => EnemyFactory._instance;

    List<string> enemyNames = new List<string>() { "Devotee", "Fanatic" };

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        jsonReader = DemoManager.Instance.jsonReader;
    }

    public List<BaseEnemy> GenerateRandomEnemies(int numberOfEnemies)
    {
        List<BaseEnemy> enemies = new List<BaseEnemy>();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            BaseEnemy enemy = GenerateRandomEnemy();

            /* Hard coded due to GigDemo models and anims being hard coded*/
            if (i == 0)
            {
                enemy.Name = "Devotee";
            }
            if (i == 1)
            {
                enemy.Name = "Fanatic";
            }

            enemies.Add(enemy);
        }
        return enemies;
    }

    public BaseEnemy GenerateRandomEnemy()
    {
        
        // Retrieve reference to the player
        List<BasePlayer> party = PartyManager.Instance.currentParty;
        BasePlayer player = new BasePlayer();
        foreach (BasePlayer p in party)
        {
            if (p.ID == 0)
            {
                player = p;
            }
        }

        // Generate random health (-7 to 0 that of the player)
        int health = Random.Range(player.Health - 7, player.Health);
        // Generate random health (-5 to 0 that of the player)
        int flourish = Random.Range(player.Flourish - 5, player.Flourish);
        int shield = 0;

        // Generate random name
        // int nameIndex = Random.Range(0, enemyNames.Count - 1);

        return new BaseEnemy(enemyNames[0], health, flourish, shield, jsonReader.baseClasses.GetRandomClass());
    }
}
