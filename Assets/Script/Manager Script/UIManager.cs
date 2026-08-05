using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject pauseUI;

    [Header("References")]
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text pauseScoreText;
    [SerializeField] private TMP_Text gameOverScoreText;

    [Header("Button")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button pauseRestartButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    [Header("HealthBar")]
    [SerializeField] private Gradient healthBarColor;

    
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
    //    ToggleGameOverScreen(true);
    //    gameOverScoreText.text = "Score : " +  GameController.instance.playerScore;
    //    Debug.Log("Player ki hatya ho gyi hai");        
    //}
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener(() => StartGame());
        pauseButton.onClick.AddListener(() => PauseGame());
        resumeButton.onClick.AddListener(() => ResumeGame());
        pauseRestartButton.onClick.AddListener(() => RestartGame());
        restartButton.onClick.AddListener(() => RestartGame());

        //Turn on Start UI
        startGameUI.SetActive(true);
        ToggleGameOverScreen(false);
        gameUI.SetActive(false);
        pauseUI.SetActive(false);


        scoreText.text = GameController.instance.playerScore.ToString();
        SetPlayerHealth(PlayerHealth.instance.GetPlayerHealthRatio());
        playerHealthImage.color = healthBarColor.Evaluate(1);
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void IncreaseScoreCount()
    {
        scoreText.text = GameController.instance.playerScore.ToString();
    }

    public void SetPlayerHealth(float fillAmount)
    {
        playerHealthImage.fillAmount = fillAmount;
        playerHealthImage.color = healthBarColor.Evaluate(fillAmount);
    }

    void ToggleGameOverScreen(bool mode)
    {
        gameOverUI.SetActive(mode);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PauseGame()
    {
        gameUI.SetActive(false);
        pauseUI.SetActive(true);
        pauseScoreText.text = "SCORE : " + GameController.instance.playerScore;
        Time.timeScale = 0f;
    } 

    void ResumeGame()
    {
        gameUI.SetActive(true);
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void StartGame()
    {
        startGameUI.SetActive(false);
        gameUI.SetActive(true);
        GameController.instance.playGame = true;
        Time.timeScale = 1f;
    }

    public void OnPlayerDied()
    {
        ToggleGameOverScreen(true);
        gameOverScoreText.text = "SCORE : " + GameController.instance.playerScore;
        Debug.Log("Player ki hatya ho gyi hai");
    }
}
