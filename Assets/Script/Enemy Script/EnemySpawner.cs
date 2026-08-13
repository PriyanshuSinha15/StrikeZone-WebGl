using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [Header("Enemy")]
    public GameObject enemyPrefab;
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();
    public int enemySpawnedCount;
    public int enemiesLeftCount;
    public int totalEnemies;

    [Header("Spawn Properties")]
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private float spawnTime;
    private float currentTime;
    public float totalTime;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (GameController.instance.playGame)
        {
            transform.position = PlayerController.instance.transform.position;
            SetSpawnTime();

            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                // Testing total enemies count
                if(enemySpawnedCount < totalEnemies)
                {
                    int randIndex = Random.Range(0, spawnPoint.Length);
                    GameObject enemy = Instantiate(enemyPrefab, spawnPoint[randIndex].position, Quaternion.identity);
                    enemyList.Add(enemy);
                    enemySpawnedCount++;
                    currentTime = spawnTime;
                }
            }
        }

        // Game Over Logic
        if (GameController.instance.gameOver)
        {
            DestroyAllEnemies();
        }
    }

    void SetSpawnTime()
    {
        switch (GameController.instance.currentLevel)
        {
            case 1:
                spawnTime = GameController.instance.level1_SO.spawnTime;
                break;
            case 2:
                spawnTime = GameController.instance.level2_SO.spawnTime;
                break;
            case 3:
                spawnTime = GameController.instance.level3_SO.spawnTime;
                break;
            default:
                spawnTime = GameController.instance.level1_SO.spawnTime;
                break;
        }
    }

    public void DestroyAllEnemies()
    {
        if (enemyList.Count > 0)
        {
            foreach (GameObject enemy in enemyList)
            {
                Destroy(enemy);
            }

            enemyList.Clear();
        }
    }
}
