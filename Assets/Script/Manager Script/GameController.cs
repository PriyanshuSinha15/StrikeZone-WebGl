using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Test Boolean")]
    public bool startGame;
    public bool restartGame;
    public bool pauseGame;
    public bool resumeGame;

    [Header("Score")]
    public int playerScore;

    [Header("Booleans")]
    public bool gameOver;
    public bool playGame;


    [Header("Player References")]
    public GameObject player;
    public Transform playerSpawnPoint;

    [Header("Enemy References")]
    public EnemySpawner enemySpawner;

    [Header("Bullet References")]
    public List<GameObject> playerBulletList = new List<GameObject>();
    public List<GameObject> enemyBulletList  = new List<GameObject>();

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
        //PlayerHealth.instance.onPlayerDied += Player_onPlayerDied;
    }

    //Not Working Correctly
    //private void OnEnable()
    //{
    //    PlayerHealth.instance.onPlayerDied += Player_onPlayerDied;
    //}

    //private void OnDisable()
    //{
    //    PlayerHealth.instance.onPlayerDied -= Player_onPlayerDied;
    //}

    //private void Player_onPlayerDied(object sender, System.EventArgs e)
    //{
    //    Time.timeScale = 0f;
    //}

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
        #region Testing
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
        #endregion Testing
    }

    public void OnPlayerDied()
    {
        if (gameOver)
        {
            player.SetActive(false);
            Debug.Log("Mar gya hu");

            DestroyBullet();

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
        DestroyBullet();
        enemySpawner.DestroyAllEnemies();
        ResetGame();
    }

    public void ResetGame()
    {
        playerScore = 0;

        gameOver = false;
        playGame = true;

        player.SetActive(true);
        player.transform.position = playerSpawnPoint.position;

        GameplayUIManager.instance.ToggleGameUIScreen(true);
        GameplayUIManager.instance.SetScoreCount();
        PlayerHealth.instance.currentHealth = 100f;
        GameplayUIManager.instance.SetPlayerHealth(PlayerHealth.instance.GetPlayerHealthRatio());
        enemySpawner.totalTime = 0f;

        PlayVlayBridge.ReportScore(playerScore);
    }

    private void DestroyBullet()
    {
        if (playerBulletList.Count > 0)
        {
            foreach (GameObject bullet in playerBulletList)
            {
                Destroy(bullet);
            }
            playerBulletList.Clear();
        }

        if (enemyBulletList.Count > 0)
        {
            foreach (GameObject bullet in enemyBulletList)
            {
                Destroy(bullet);
            }
            enemyBulletList.Clear();
        }
    }

}
