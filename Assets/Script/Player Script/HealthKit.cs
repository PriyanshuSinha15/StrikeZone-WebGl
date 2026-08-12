using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [SerializeField] private float healthBoost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerHealth>())
        {
            PlayerHealth.instance.IncreasePlayerHealth(healthBoost);
            GameplayUIManager.instance.SetPlayerHealthUI(PlayerHealth.instance.GetPlayerHealthRatio());
            Destroy(gameObject);
        }
    }
}
