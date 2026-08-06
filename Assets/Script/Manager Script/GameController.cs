using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int playerScore;
    public bool gameOver;
    public bool playGame;

    [Header("Player References")]
    public GameObject player;
    public Transform playerSpawnPoint;

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
        gameOver = false;
        playGame = false;

        InitializeGame();

        //if (!playGame)
        //{
        //    Time.timeScale = 0f;
        //}
        //else
        //{
        //    Time.timeScale = 1f;
        //}
    }

    private void InitializeGame()
    {
        playGame = true;
        Time.timeScale = 1f;

        player.SetActive(true);
        player.transform.position = playerSpawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            player.SetActive(false);
            if(playerBulletList.Count > 0)
            {
                foreach(GameObject bullet in playerBulletList)
                {
                    Destroy (bullet);
                }
                playerBulletList.Clear();
            }

            if (enemyBulletList.Count > 0)
            {
                foreach(GameObject bullet in enemyBulletList)
                {
                    Destroy(bullet);
                }
                enemyBulletList.Clear();
            }
        }
    }
}
