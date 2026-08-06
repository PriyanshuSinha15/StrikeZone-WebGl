using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    public static GameplayUIManager instance;

    [Header("UI References")]
    [SerializeField] private GameObject gameUI;

    [Header("References")]
    [SerializeField] private Image playerHealth;
    [SerializeField] private TMP_Text scoreText;

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
        scoreText.text = GameController.instance.playerScore.ToString();
        playerHealth.fillAmount = 1;
        playerHealth.color = healthBarColor.Evaluate(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScoreCount()
    {
        scoreText.text = GameController.instance.playerScore.ToString();
    }

    public void SetPlayerHealth(float fillAmount)
    {
        playerHealth.fillAmount = fillAmount;
        playerHealth.color = healthBarColor.Evaluate(fillAmount);
    }

    public void ToggleGameUIScreen(bool mode)
    {
        gameUI.SetActive(mode);
    }

    public void OnPlayerDied()
    {
        ToggleGameUIScreen(false);
    }
}
