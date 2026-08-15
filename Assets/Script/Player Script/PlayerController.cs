using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Terresquall;
using UnityEngine;
using UnityEngine.UI;
using MyTouch = UnityEngine.InputSystem.EnhancedTouch;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private InputSystem_Actions inputSystem;

    private Rigidbody2D rb;

    [Header("Input Mangnitude")]
    [SerializeField] private float shootInputMagnitude;

    [Header("Joystick")]
    [SerializeField] private VirtualJoystick movementJoystick;
    //[SerializeField] private VirtualJoystick rotationJoystick;

    [Header("Button")]
    [SerializeField] private Button shootButton;

    [Header("Player Abilities")]
    [SerializeField] private float speed;
    [SerializeField] private float enemyDetectRange;
    [SerializeField] private float spawnPointRotationRange;

    [Header("Shoot")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float reloadTimer;
    private float currentTimer;

    [Header("Enemy")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float enemyDetectInterval;
    private bool enemyDetected;
    private float enemyDetectTimer;

    [Header("Move Area")]
    [SerializeField] private float xMinRange;
    [SerializeField] private float xMaxRange;
    [SerializeField] private float yMinRange;
    [SerializeField] private float yMaxRange;

    private Vector2 moveDir;
    private Vector2 worldMousePosition;
    private bool hasRotationInput;
    private readonly HashSet<int> joystickTouchIds = new HashSet<int>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void ShootBullet()
    {
        currentTimer -= Time.deltaTime;

        if(currentTimer <= 0 && enemyDetected)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            bullet.GetComponent<BulletScript>().ShootBullet(bulletSpeed);

            SoundManager.instance.PlaySound(0);
            GameController.instance.playerBulletList.Add(bullet);

            StartCoroutine(DestroyBullet(bullet, 3f));

            currentTimer = reloadTimer;
        }
    }
    void Start()
    {
        //shootButton.onClick.AddListener(() => ShootBullet());
        enemyDetectTimer = 0;
        currentTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameInput();
        RestrictPlayerPosition();
    }

    private void FixedUpdate()
    {
        enemyDetectTimer -= Time.fixedDeltaTime;

        if(enemyDetectTimer <= 0)
        {
            DetectEnemies();
            enemyDetectTimer = enemyDetectInterval;
        }
        MoveAndRotatePlayer();

        ShootBullet();
    }

    private void GameInput()
    {
        //moveDir = inputSystem.Player.Move.ReadValue<Vector2>();

        // Movement Input
        moveDir.x = movementJoystick.GetAxis("Horizontal");
        moveDir.y = movementJoystick.GetAxis("Vertical");
        moveDir.Normalize();

        // Rotation
        HandleRotation();

        //Vector2 mousePosition = inputSystem.UI.Point.ReadValue<Vector2>();
        //worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    private void HandleRotation()
    {
        foreach (var touch in MyTouch.Touch.activeTouches)
        {
            hasRotationInput = false;

            int fingerId = touch.finger.index;

            // =========================================================
            // 1. NEW TOUCH
            // =========================================================
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                // If the touch started in the joystick's interaction area,
                // permanently classify this finger as a joystick finger.
                if (IsTouchOnJoystick(touch.screenPosition))
                {
                    joystickTouchIds.Add(fingerId);
                }
            }

            // =========================================================
            // 2. TOUCH RELEASE
            // =========================================================
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                // Remove the finger BEFORE checking whether it is a
                // joystick finger.
                joystickTouchIds.Remove(fingerId);

                continue;
            }

            // =========================================================
            // 3. IGNORE JOYSTICK FINGER
            // =========================================================
            if (joystickTouchIds.Contains(fingerId))
            {
                continue;
            }

            // =========================================================
            // 4. THIS IS A ROTATION TOUCH
            // =========================================================
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                Vector2 screenPosition = touch.screenPosition;

                float distanceFromCamera =
                    Mathf.Abs(
                        Camera.main.transform.position.z -
                        transform.position.z
                    );

                worldMousePosition = Camera.main.ScreenToWorldPoint(
                    new Vector3(
                        screenPosition.x,
                        screenPosition.y,
                        distanceFromCamera
                    )
                );

                hasRotationInput = true;
            }
        }
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

        // Don't change the rotation unless player touched outside Joystick
        if (!hasRotationInput)
            return;

        Vector2 lookDir = worldMousePosition - rb.position;

        Vector3 worldMousePosition_3dPosition = new Vector3(worldMousePosition.x, worldMousePosition.y, 0);

        Vector2 spawnPointLookDir = worldMousePosition_3dPosition - spawnPoint.transform.position;
        Vector2 spawnPointLookDirNormalized = spawnPointLookDir.normalized;
        
        //Vector2 lookDir = new Vector2(rotationJoystick.GetAxis("Horizontal"), rotationJoystick.GetAxis("Vertical"));
        Vector2 lookDirNormalized = lookDir.normalized;

        // Player Rotation
        if (lookDir.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(lookDirNormalized.y, lookDirNormalized.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }

        //SpawnPoint Rotation
        if (Vector3.Distance(worldMousePosition_3dPosition, transform.position) > spawnPointRotationRange)
        {
            if(spawnPointLookDir.sqrMagnitude > 0.01f)
            {
                float spawnPointRotAngle = Mathf.Atan2(spawnPointLookDirNormalized.y, spawnPointLookDirNormalized.x) * Mathf.Rad2Deg - 90f;
                spawnPoint.transform.eulerAngles = new Vector3(0, 0, spawnPointRotAngle);
            }
        }
        else
        {
            spawnPoint.transform.localEulerAngles = new Vector3(0, 0, 0);
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

    void DetectEnemies()
    {
        Collider2D enemy = Physics2D.OverlapCircle(transform.position, enemyDetectRange, enemyLayer);

        enemyDetected = enemy != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnPointRotationRange);
    }

    public void ResetPlayerInput()
    {
        hasRotationInput = false;
        worldMousePosition = rb.position;

        joystickTouchIds.Clear();

        rb.rotation = 0f;
        spawnPoint.localEulerAngles = Vector3.zero;
    }

    private bool IsTouchOnJoystick(Vector2 screenPosition)
    {
        // ---------------------------------------------------------
        // If Terresquall Snap To Touch is enabled,
        // use the joystick's actual interaction boundaries.
        // ---------------------------------------------------------
        Rect snapBounds = movementJoystick.GetBounds();

        if (snapBounds.width > 0f && snapBounds.height > 0f)
        {
            return snapBounds.Contains(screenPosition);
        }

        // ---------------------------------------------------------
        // Normal fixed joystick:
        // use the visible joystick RectTransform.
        // ---------------------------------------------------------
        RectTransform joystickRect =
            movementJoystick.GetComponent<RectTransform>();

        return RectTransformUtility.RectangleContainsScreenPoint(
            joystickRect,
            screenPosition,
            null
        );
    }

    public void ResetTimer()
    {
        currentTimer = 0;
        enemyDetectTimer = 0;
    }
}
