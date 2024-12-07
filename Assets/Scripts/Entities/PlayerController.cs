using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private bool inputEnabled;
    [SerializeField] private InputActionReference slowMode;
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference touchMove;
    [SerializeField] private InputActionReference shoot;
    [SerializeField] private InputActionReference isTouching;

    [Header("Player")]
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField, Range(0.0f, 1.0f)] private float slowSpeedMult = 0.5f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;

    [Header("Getting Hit")]
    [SerializeField] private Color hurtColor;
    [SerializeField] private float invincibilityDuration = 0.5f;
    [SerializeField] private Transform respawnPoint;
    //[SerializeField] private int startingLives = 2;
    [SerializeField] private int lifeEssenceCost = 40;

    [Header("READ ONLY")]
    [SerializeField] public int currentLives;
    [SerializeField] public int totalHitsTaken;
    [SerializeField] public int totalCollectedEssence;
    [SerializeField] public int essenceTowardsNextLife;

    private bool slowModeActivated;

    public float SpeedMult => slowModeActivated ? slowSpeedMult : 1.0f;

    private Rigidbody2D rb;

    private float fireTimer;
    private float invincibilityTimer;

    public UnityEvent OnDeath;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ResetData();
        slowMode.action.Enable();
        move.action.Enable();
        touchMove.action.Enable();
        shoot.action.Enable();
        isTouching.action.Enable();
    }

    public void ResetData()
    {
        totalHitsTaken = 0;
        totalCollectedEssence = 0;
        fireTimer = 0.0f;
        invincibilityTimer = 0.0f;
    }

    public void SetEnableInputs(bool enable)
    {
        Debug.Log($"Enabled Inputs: {enable}");
        inputEnabled = enable;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Inputs
        if (inputEnabled)
        {
            
            slowModeActivated = slowMode.action.IsPressed();
            Vector2 moveDir = GetVelocityFromInput();

            if (shoot.action.IsPressed())
            {
                Fire();
            }

            Vector2 vel = moveDir * baseSpeed * SpeedMult;
            rb.velocity = vel;
        } else
        {
            rb.velocity = Vector2.zero;
        }

        
        if (invincibilityTimer > 0.0f)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    private Vector2 GetVelocityFromInput()
    {
        //float xVel, yVel;
        Vector2 moveInput;

        if (isTouching.action.IsPressed())
        {
            Vector2 position = touchMove.action.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);

            moveInput = ((Vector2)worldPos - (Vector2)transform.position);

            //Set a threshold for when the player should move vs. stay still
            moveInput = moveInput.magnitude > 0.05f ? moveInput.normalized : Vector2.zero;
        } else if (move.action.IsInProgress())
        {
            moveInput = move.action.ReadValue<Vector2>().normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }

        return moveInput;
    }

    public void Damage()
    {
        if (invincibilityTimer <= 0.0f)
        {
            totalHitsTaken++;
            GetComponentInChildren<SpriteFlasher>()?.Flash(hurtColor, invincibilityDuration);

            invincibilityTimer = invincibilityDuration;
        }
    }

    public void Kill()
    {
        Debug.Log("KILLED PLAYER");
        SetEnableInputs(false);
        OnDeath?.Invoke();

        //Decide what to do when player dies.
    }

    public void Respawn()
    {
        Debug.Log("RESPAWNED PLAYER");
        transform.position = respawnPoint.position;
        invincibilityTimer = invincibilityDuration;
        totalCollectedEssence = 0;

        SetEnableInputs(true);
    }

    public void AddEssence(int amount)
    {
        totalCollectedEssence += amount;
        essenceTowardsNextLife += amount;
        if (essenceTowardsNextLife > lifeEssenceCost)
        {
            currentLives++;
            essenceTowardsNextLife -= lifeEssenceCost;

            //Flash the essence bar or smth.
        }
        //Update the essence bar UI.
    }

    public void FadeIn(float duration = 0.5f)
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            sr.DOColor(new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f), duration);
        }
    }

    public void FadeOut(float duration = 0.5f)
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            sr.DOColor(new Color(sr.color.r, sr.color.g, sr.color.b, 0.0f), duration);
        }
    }

    public void Fire()
    {
        if (fireTimer <= 0.0f)
        {
            //Fire a new bullet
            GameObject newBulletObj = GameObject.Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity);
            Bullet bullet = newBulletObj.GetComponent<Bullet>();
            bullet.SetVelocity(Vector2.up * bulletSpeed);
            fireTimer = 1f / fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }
    }
}
