using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float bulletDamage;

    private Coroutine lifeTimeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        lifeTimeCoroutine = StartCoroutine(BulletLifeTime());
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if(lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }
    }

    public void ShootBullet(float bulletSpeed)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == PlayerController.instance.gameObject)
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(bulletDamage);
            GameplayUIManager.instance.SetPlayerHealthUI(PlayerHealth.instance.GetPlayerHealthRatio());
            ReturnToPool();
        }
    }

    private IEnumerator BulletLifeTime()
    {
        yield return new WaitForSeconds(3f);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        GameController.instance.enemyBulletList.Remove(gameObject);

        EnemyBulletPool.instance.ReturnBullet(gameObject);
    }


}
