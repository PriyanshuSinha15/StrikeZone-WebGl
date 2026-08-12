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

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed;
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
        if(Vector3.Distance(transform.position, player.position) >= attackDistance)
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

            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            bullet.GetComponent<EnemyBullet>().ShootBullet(bulletSpeed);
            SoundManager.instance.PlaySound(2);
            GameController.instance.enemyBulletList.Add(bullet);
            StartCoroutine(DestroyBullet(bullet, 3f));
            currentTimer = reloadTimer;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BulletScript>())
        {
            GameController.instance.playerScore += 2;

            //PlayVlay Score Report
            PlayVlayBridge.ReportScore(GameController.instance.playerScore);

            GameplayUIManager.instance.SetScoreCount();
            SoundManager.instance.PlaySound(1);
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 1.2f);

            GameController.instance.playerBulletList.Remove(collision.gameObject);
            Destroy(collision.gameObject);

            GameObject healthKit = Instantiate(GameController.instance.healthKit, transform.position, Quaternion.identity);
            Destroy(healthKit, 20f);

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

    IEnumerator DestroyBullet(GameObject bullet, float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);

        if(bullet != null)
        {
            GameController.instance.enemyBulletList.Remove(bullet);
            Destroy(bullet);
        }
    }
}
