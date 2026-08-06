using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private float spawnTime;
    private float currentTime;
    public float totalTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetSpawnTime();

        currentTime -= Time.deltaTime;
        if(currentTime <= 0)
        {
            int randIndex = Random.Range(0, spawnPoint.Length);
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint[randIndex].position, Quaternion.identity);
            enemyList.Add(enemy);
            currentTime = spawnTime;
        }

        // Game Over Logic
        if (GameController.instance.gameOver)
        {
            DestroyAllEnemies();
        }
    }

    void SetSpawnTime()
    {
        totalTime += Time.deltaTime;

        if(totalTime <= 15)
        {
            spawnTime = 4f;
        }
        else if(totalTime > 15 && totalTime < 50)
        {
            spawnTime = 3f;
        }
        else if(totalTime > 50 && totalTime < 90)
        {
            spawnTime = 2f;
        }
        else
        {
            spawnTime = 1.5f;
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
