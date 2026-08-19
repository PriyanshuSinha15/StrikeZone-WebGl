using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private InputSystem_Actions inputSystem;

    [Header("Test Boolean")]
    public bool startGame;
    public bool restartGame;
    public bool pauseGame;
    public bool resumeGame;
    public bool reportScore;

    [Header("Score")]
    public int playerScore;

    [Header("Booleans")]
    public bool gameOver;
    public bool playGame;

    [Header("Level References")]
    public int currentLevel;

    [Header("Level SO")]
    public Level_SO level1_SO;
    public Level_SO level2_SO;
    public Level_SO level3_SO;

    [Header("Player References")]
    public GameObject player;
    public Transform playerSpawnPoint;

    [Header("HealthKit")]
    public GameObject healthKit;

    [Header("List References")]
    public List<GameObject> playerBulletList = new List<GameObject>();
    public List<GameObject> enemyBulletList  = new List<GameObject>();
    public List<GameObject> healthKitList = new List<GameObject>();
    public List<GameObject> explosionPrefabList = new List<GameObject>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inputSystem = new InputSystem_Actions();
        //PlayerHealth.instance.onPlayerDied += Player_onPlayerDied;
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Jump.performed += Start_performed;
        inputSystem.Player.Sprint.performed += Restart_performed;
    }


    private void OnDisable()
    {
        inputSystem.Disable();
        inputSystem.Player.Jump.performed -= Start_performed;
        inputSystem.Player.Sprint.performed -= Restart_performed;
    }

    private void Restart_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        RestartGame();
    }
    private void Start_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StartGame();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartGame();
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //TESTING FUNCTIONALITY

        TestingBooleans();

        SetCurrentLevelProperties();
    }

    #region Testing
    private void TestingBooleans()
    {
        if (startGame)
        {
            StartGame();
            startGame = false;
        }

        if (restartGame)
        {
            RestartGame();
            restartGame = false;
        }

        if (pauseGame)
        {
            PauseGame();
            pauseGame = false;
        }

        if (resumeGame)
        {
            ResumeGame();
            resumeGame = false;
        }

        if (reportScore)
        {
            PlayVlayBridge.ReportScore(123);
            reportScore = false;
        }
    }
    #endregion Testing

    public void OnPlayerDied()
    {
        if (gameOver)
        {
            player.SetActive(false);

            ResetPlayerBullets();
            ResetEnemyBullet();
            DestroyHealthKit();
            DestroyExplosionPrefabs();

            PlayVlayBridge.GameOver(playerScore);
            playGame = false;
            Time.timeScale = 0f;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f; 

        ResetGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        ResetPlayerBullets();
        ResetEnemyBullet();
        DestroyHealthKit();
        DestroyExplosionPrefabs();
        EnemySpawner.instance.DestroyAllEnemies();
        ResetGame();
    }

    public void ResetGame()
    {
        playerScore = 0;

        gameOver = false;
        playGame = true;

        player.SetActive(true);
        player.transform.position = playerSpawnPoint.position;

        //Reseting player input
        PlayerController.instance.ResetPlayerInput();

        GameplayUIManager.instance.ToggleGameUIScreen(true);
        GameplayUIManager.instance.SetScoreCountUI();
        PlayerHealth.instance.currentHealth = PlayerHealth.instance.totalHealth;

        //Resetting Player Timer
        PlayerController.instance.ResetTimer();

        //Testing Enemies Count
        EnemySpawner.instance.enemiesLeftCount = level1_SO.enemyCount;
        EnemySpawner.instance.enemyPrefab = level1_SO.enemyPrefab;
        EnemySpawner.instance.enemySpawnedCount = 0;
        GameplayUIManager.instance.SetEnemiesLeftCountUI();

        GameplayUIManager.instance.SetPlayerHealthUI(PlayerHealth.instance.GetPlayerHealthRatio());
        EnemySpawner.instance.totalTime = 0f;

        //Reset Level
        currentLevel = 1;
        GameplayUIManager.instance.SetCurrentLevelUI();

        PlayVlayBridge.ReportScore(playerScore);
    }

    private void ResetPlayerBullets()
    {
        if (playerBulletList.Count == 0)
            return;

        for(int i = playerBulletList.Count - 1; i >= 0; i--)
        {
            GameObject bullet = playerBulletList[i];

            if(bullet != null)
            {
                BulletPool.instance.ReturnBullet(bullet);
            }
        }

        playerBulletList.Clear();
    }

    private void ResetEnemyBullet()
    {
        if (enemyBulletList.Count == 0)
            return;

        for(int i = enemyBulletList.Count - 1; i>=0; i--)
        {
            GameObject bullet = enemyBulletList[i];

            if(bullet != null)
            {
                EnemyBulletPool.instance.ReturnBullet(bullet);
            }
        }

        enemyBulletList.Clear();
        
    }

    private void DestroyHealthKit()
    {
        if(healthKitList.Count > 0)
        {
            foreach(GameObject healthkit in healthKitList)
            {
                Destroy(healthkit);
            }
            healthKitList.Clear();
        }
    }

    private void DestroyExplosionPrefabs()
    {
        if(explosionPrefabList.Count > 0)
        {
            foreach(GameObject explosionPrefab in explosionPrefabList)
            {
                Destroy(explosionPrefab);
            }
            explosionPrefabList.Clear();
        }
    }

    private void SetCurrentLevelProperties()
    {
        switch (currentLevel)
        {
            case 1:
                EnemySpawner.instance.totalEnemies = level1_SO.enemyCount;
                EnemySpawner.instance.enemyPrefab = level1_SO.enemyPrefab;

                if(EnemySpawner.instance.enemiesLeftCount == 0)
                {
                    EnemySpawner.instance.enemySpawnedCount = 0;
                    currentLevel = 2;
                    EnemySpawner.instance.enemiesLeftCount = level2_SO.enemyCount;
                    GameplayUIManager.instance.SetCurrentLevelUI();
                    GameplayUIManager.instance.SetEnemiesLeftCountUI();
                }
                break;
            case 2:
                EnemySpawner.instance.totalEnemies = level2_SO.enemyCount;
                EnemySpawner.instance.enemyPrefab = level2_SO.enemyPrefab;

                if (EnemySpawner.instance.enemiesLeftCount == 0)
                {
                    EnemySpawner.instance.enemySpawnedCount = 0;
                    currentLevel = 3;
                    EnemySpawner.instance.enemiesLeftCount = level3_SO.enemyCount;
                    GameplayUIManager.instance.SetCurrentLevelUI();
                    GameplayUIManager.instance.SetEnemiesLeftCountUI();
                }
                break;

            case 3:
                EnemySpawner.instance.totalEnemies = level3_SO.enemyCount;
                EnemySpawner.instance.enemyPrefab = level3_SO.enemyPrefab;

                if (EnemySpawner.instance.enemiesLeftCount == 0)
                {
                    RestartGame();
                }
                break;

            default:
                EnemySpawner.instance.totalEnemies = level1_SO.enemyCount;
                EnemySpawner.instance.enemyPrefab = level1_SO.enemyPrefab;
                break;
        }
    }

}
