using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int playerScore;
    public bool gameOver;
    public bool playGame;

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

        if (!playGame)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
