using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    //public event EventHandler onPlayerDied;

    [SerializeField] private float totalHealth;
    public float currentHealth;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            GameController.instance.gameOver = true;
            GameplayUIManager.instance.OnPlayerDied();
            GameController.instance.OnPlayerDied();
            
            //onPlayerDied?.Invoke(this, EventArgs.Empty);

        }
    }

    public float GetPlayerHealthRatio()
    {
        float fillAmount = currentHealth / totalHealth;
        return fillAmount;
    }
}
