using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public enum EnemyState
{
    CHASE,
    ATTACK
}
public class EnemyController : MonoBehaviour
{
    public EnemyState state;
    private Transform player;
    private Rigidbody2D rb;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("Enemy Abilities")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private float reloadTimer;
    [SerializeField] private float decisionTimer;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed;

    private float currentDecisionTimer;
    private float currentTimer;
    private Vector2 lookDir;
    private float lookAngle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        player = PlayerController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentDecisionTimer -= Time.deltaTime;

        if(currentDecisionTimer < 0)
        {
            UpdateEnemyState();
            currentDecisionTimer = decisionTimer;
        }
    }

    private void UpdateEnemyState()
    {
        if (Vector2.Distance(transform.position, player.position) >= attackDistance)
        {
            state = EnemyState.CHASE;
        }
        else
        {
            state = EnemyState.ATTACK;
        }
    }

    void FixedUpdate()
    {
        EnemyRotation();
        Vector2 targetDir = lookDir.normalized;
        spawnPoint.eulerAngles = new Vector3(0, 0, SpawnPointRotAngle());

        if (state == EnemyState.CHASE)
        {

        }
        else if (state == EnemyState.ATTACK)
        {
            targetDir = Vector2.zero;
            ShootBullet();
        }

        rb.linearVelocity = targetDir * enemySpeed;
    }

    private void ShootBullet()
    {
        currentTimer -= Time.fixedDeltaTime;
        if(currentTimer <= 0)
        {

            GameObject bullet = EnemyBulletPool.instance.GetBullet(spawnPoint.position, spawnPoint.rotation);
            bullet.GetComponent<EnemyBullet>().ShootBullet(bulletSpeed);
            SoundManager.instance.PlaySound(2);
            GameController.instance.enemyBulletList.Add(bullet);
  
            currentTimer = reloadTimer;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BulletScript>())
        {
            GameController.instance.playerScore += GameController.instance.playerScoreBoost;

            //PlayVlay Score Report
            PlayVlayBridge.ReportScore(GameController.instance.playerScore);

            GameplayUIManager.instance.SetScoreCountUI();
            SoundManager.instance.PlaySound(1);

            // Getting Explosion from pool 
            GameObject explosion = ExplosionPool.instance.GetExplosion(transform.position, Quaternion.identity);
            GameController.instance.explosionPrefabList.Add(explosion);

            GameController.instance.playerBulletList.Remove(collision.gameObject);
            BulletPool.instance.ReturnBullet(collision.gameObject);

            //Enemies Left Count Test
            EnemySpawner.instance.enemiesLeftCount--;
            GameplayUIManager.instance.SetEnemiesLeftCountUI();

            //Health Kit Power up
            GameObject healthKit = Instantiate(GameController.instance.healthKit, transform.position, Quaternion.identity);
            GameController.instance.healthKitList.Add(healthKit);
            Destroy(healthKit, 20f);

            // Remove enemy from Enemy List in Enemy Spawner
            EnemySpawner.instance.enemyList.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    void EnemyRotation()
    {
        lookDir = player.position - transform.position;
        lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = lookAngle;
    }

    float SpawnPointRotAngle()
    {
        Vector2 lookDir = player.position - spawnPoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        return angle; 
    }
}
