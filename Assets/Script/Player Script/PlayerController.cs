using NUnit.Framework;
using System;
using System.Collections;
using Terresquall;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private InputSystem_Actions inputSystem;

    private Rigidbody2D rb;

    [Header("Joystick")]
    [SerializeField] private VirtualJoystick movementJoystick;
    //[SerializeField] private VirtualJoystick rotationJoystick;

    [Header("Button")]
    [SerializeField] private Button shootButton;

    [Header("Player Abilities")]
    [SerializeField] private float speed;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed;

    [Header("Move Area")]
    [SerializeField] private float xMinRange;
    [SerializeField] private float xMaxRange;
    [SerializeField] private float yMinRange;
    [SerializeField] private float yMaxRange;

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
        //inputSystem.Player.Attack.performed += Attack_performed;
    }

    //private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{
    //    GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
    //    bullet.GetComponent<BulletScript>().ShootBullet(bulletSpeed);

    //    SoundManager.instance.PlaySound(0);

    //    Destroy(bullet, 3f);
    //}

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<BulletScript>().ShootBullet(bulletSpeed);

        SoundManager.instance.PlaySound(0);
        GameController.instance.playerBulletList.Add(bullet);

        StartCoroutine(DestroyBullet(bullet, 3f));
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
    void Start()
    {
        shootButton.onClick.AddListener(() => ShootBullet());
    }

    // Update is called once per frame
    void Update()
    {
        GameInput();
        RestrictPlayerPosition();
    }

    private void FixedUpdate()
    {
        MoveAndRotatePlayer();
    }

    private void GameInput()
    {
        //moveDir = inputSystem.Player.Move.ReadValue<Vector2>();

        moveDir.x = movementJoystick.GetAxis("Horizontal");
        moveDir.y = movementJoystick.GetAxis("Vertical");
        moveDir.Normalize();

        Vector2 mousePosition = inputSystem.UI.Point.ReadValue<Vector2>();
        worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void RestrictPlayerPosition()
    {
        float xPosition = Mathf.Clamp(transform.position.x, xMinRange, xMaxRange);
        float yPosition = Mathf.Clamp(transform.position.y, yMinRange, yMaxRange);

        transform.position = new Vector2(xPosition, yPosition);
    }

    private void MoveAndRotatePlayer()
    {
        rb.linearVelocity = moveDir * speed;

        Vector2 lookDir = worldMousePosition - rb.position;
        //Vector2 lookDir = new Vector2(rotationJoystick.GetAxis("Horizontal"), rotationJoystick.GetAxis("Vertical"));
        lookDir.Normalize();

        if(lookDir.sqrMagnitude > 0.01)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }

    IEnumerator DestroyBullet(GameObject bullet, float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);

        if(bullet != null)
        {
            GameController.instance.playerBulletList.Remove(bullet);
            Destroy(bullet);
        }
    }
}
