using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TMP_Text scoreText; 
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
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "Score : " + GameController.instance.playerScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseScoreCount()
    {
        scoreText.text = "Score : " + GameController.instance.playerScore;
    }
}
