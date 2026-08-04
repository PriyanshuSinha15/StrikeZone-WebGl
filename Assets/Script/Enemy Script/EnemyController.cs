using UnityEngine;

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

    [Header("Enemy Abilities")]
    [SerializeField] private float enemySpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private float reloadTimer;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed;
    private float currentTimer;
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
        Vector2 lookDir = player.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        Vector2 targetDir = lookDir.normalized;
        
       if(state == EnemyState.CHASE)
       {

       } 
       else if(state == EnemyState.ATTACK)
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
            Destroy(bullet, 3f);
            currentTimer = reloadTimer;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BulletScript>())
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
