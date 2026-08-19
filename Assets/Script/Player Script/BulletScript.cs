using System;
using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Coroutine lifetimeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // Reset any velocity left from previous one
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // Start Lifetime every time the bullet is activated
        lifetimeCoroutine = StartCoroutine(BulletLifetime());

    }

    void OnDisable()
    {
        if(lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
            lifetimeCoroutine = null;
        }

        // Make sure the pooled bullet is completely stopped
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    public void ShootBullet(float bulletSpeed)
    {
        rb.linearVelocity = Vector2.zero;

        rb.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private IEnumerator BulletLifetime()
    {
        yield return new WaitForSeconds(3f);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if(GameController.instance.playerBulletList.Contains(gameObject))
        {
            GameController.instance.playerBulletList.Remove(gameObject);
        }

        BulletPool.instance.ReturnBullet(gameObject);
    }
}
