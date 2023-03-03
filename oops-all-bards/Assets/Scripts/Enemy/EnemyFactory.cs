using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private JSONReader jsonReader;

    private static EnemyFactory _instance;
    public static EnemyFactory Instance => EnemyFactory._instance;

    [SerializeField]
    private List<GameObject> enemyModelPrefabs;

    [SerializeField]
    private List<Transform> enemyTransforms;

    [SerializeField]
    RuntimeAnimatorController runtimeAnimatorController;

    public int enemyNumber;

    List<string> enemyNames = new List<string>() { "Devotee", "Fanatic", "Advocate", "Recruit", "Follower" };
    List<string> enemyNamesTaken = new List<string>();
 
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            jsonReader = DemoManager.Instance.jsonReader;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    public List<BaseEnemy> GenerateRandomEnemies(int numberOfEnemies)
    {
        List<BaseEnemy> enemies = new List<BaseEnemy>();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            BaseEnemy enemy = GenerateRandomEnemy();

            /* Hard coded due to GigDemo models and anims being hard coded*/
            /* if (i == 0)
             {
                 enemy.Name = "Devotee";
             }
             if (i == 1)
             {
                 enemy.Name = "Fanatic";
             }*/

            SpawnEnemyModel(enemy.Name);
            enemies.Add(enemy);
            enemyNumber++;
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

        string enemyName = GenerateUniqueEnemyName();

        return new BaseEnemy(enemyName, health, flourish, shield, jsonReader.baseClasses.GetRandomClass());
    }

    private string GenerateUniqueEnemyName()
    {
        // Generate random name
        int nameIndex = Random.Range(0, enemyNames.Count - 1);

        string enemyName = enemyNames[nameIndex];

        for (int i = 0; i < enemyNamesTaken.Count; i++)
        {
            if (enemyName == enemyNamesTaken[i])
            {
                nameIndex = Random.Range(0, enemyNames.Count - 1);
                enemyName = enemyNames[nameIndex];
                i = -1; // restart the loop
            }
        }
        enemyNamesTaken.Add(enemyName);
        return enemyName;
    }

    public void SpawnEnemyModel(string enemyName)
    {
        int modelIndex = Random.Range(0, enemyModelPrefabs.Count - 1);
        GameObject enemyModel = Instantiate(enemyModelPrefabs[modelIndex]);

        enemyModel.name = enemyName + "Model";
        enemyModel.transform.position = enemyTransforms[enemyNumber].position;
        enemyModel.transform.localScale = new Vector3(2, 2, 2);

        enemyModel.transform.Rotate(new Vector3(0, -90, 0));

        Animator animator = enemyModel.GetComponent<Animator>();
        if (animator == null) return;

        animator.runtimeAnimatorController = runtimeAnimatorController;
        animator.applyRootMotion = false;
    }

}
