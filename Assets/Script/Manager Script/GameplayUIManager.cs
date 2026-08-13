using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager instance;

    [Header("Button")]
    [SerializeField] private Button startButton;

    [Header("UI References")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject goalUI;

    [Header("Test References")]
    [SerializeField] private bool test;
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject retryGameUI;

    [Header("References")]
    [SerializeField] private Image playerHealth;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text enemiesLeftCount;
    [SerializeField] private TMP_Text currentLevelText;

    [Header("HealthBar")]
    [SerializeField] private Gradient healthBarColor;

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
        //TEST Code
        if (test)
        {
            //startGameUI.SetActive(true);
        }
        else
        {
            //startGameUI.SetActive(false);
        }
        //TEST Code

        startButton.onClick.AddListener(() => StartGameProcess());

        goalUI.SetActive(true);
        scoreText.text = GameController.instance.playerScore.ToString();
        playerHealth.fillAmount = 1;
        playerHealth.color = healthBarColor.Evaluate(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScoreCountUI()
    {
        scoreText.text = GameController.instance.playerScore.ToString();
    }

    public void SetPlayerHealthUI(float fillAmount)
    {
        playerHealth.fillAmount = fillAmount;
        playerHealth.color = healthBarColor.Evaluate(fillAmount);
    }

    public void SetEnemiesLeftCountUI()
    {
        enemiesLeftCount.text = "Enemies Left : " +  EnemySpawner.instance.enemiesLeftCount;
    }

    public void SetCurrentLevelUI()
    {
        currentLevelText.text = "PHASE " + GameController.instance.currentLevel;
    } 

    public void ToggleGameUIScreen(bool mode)
    {
        gameUI.SetActive(mode);
    }

    public void OnPlayerDied()
    {
        ToggleGameUIScreen(false);

        //TEST Code
        if (test)
        {
            retryGameUI.SetActive(true);
        }
    }

    //Test CODE 
    public void StartGameProcess()
    {
        goalUI.SetActive(false);
        GameController.instance.StartGame();
    }

    public void RestartGameTest()
    {
        if (test)
        {
            GameController.instance.RestartGame();
            retryGameUI.SetActive(false);
        }
    }
}
