using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float spawnTime;
    private float currentTime;

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
            currentTime = spawnTime;
        }
    }

    void SetSpawnTime()
    {
        Debug.Log(Time.time);

        if(Time.time <= 15)
        {
            spawnTime = 4f;
        }
        else if(Time.time > 15 && Time.time < 50)
        {
            spawnTime = 3f;
        }
        else if(Time.time > 50 && Time.time < 90)
        {
            spawnTime = 2f;
        }
        else
        {
            spawnTime = 1.5f;
        }

    }
}
