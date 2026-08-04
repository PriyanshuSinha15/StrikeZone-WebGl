using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private InputSystem_Actions inputSystem;

    private Rigidbody2D rb;

    [Header("Player Abilities")]
    [SerializeField] private float speed;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed;

    [Header("Move Area")]
    [SerializeField] private float xRange;
    [SerializeField] private float yRange;

    private Vector2 moveDir;
    private Vector2 worldMousePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        inputSystem = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Attack.performed += Attack_performed;
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<BulletScript>().ShootBullet(bulletSpeed);

        SoundManager.instance.PlaySound(0);

        Destroy(bullet, 3f);
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = inputSystem.Player.Move.ReadValue<Vector2>();
        moveDir.Normalize();

        Vector2 mousePosition = inputSystem.UI.Point.ReadValue<Vector2>();
        worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        float xPosition = Mathf.Clamp(transform.position.x, -xRange, xRange);
        float yPosition = Mathf.Clamp(transform.position.y, -yRange, yRange);

        transform.position = new Vector2(xPosition, yPosition);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDir * speed;

        Vector2 lookDir = worldMousePosition - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
