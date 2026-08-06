using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float bulletDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ShootBullet(float bulletSpeed)
    {
        rb.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == PlayerController.instance.gameObject)
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            UIManager.instance.SetPlayerHealth(PlayerHealth.instance.GetPlayerHealthRatio());
            Destroy(gameObject);
        }
    }


}
