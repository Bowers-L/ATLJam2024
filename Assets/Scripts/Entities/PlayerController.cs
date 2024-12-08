using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private bool inputEnabled;
    [SerializeField] private InputActionReference slowMode;
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference shoot;

    [Header("Player")]
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField, Range(0.0f, 1.0f)] private float slowSpeedMult = 0.5f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;

    public int maxHealth = 20;

    private int health;

    public UnityEvent<int> OnMaxHealth;
    public UnityEvent<int> OnHealthChanged;

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
        //slowMode.action.Enable();
        //move.action.Enable();
        shoot.action.Enable();
    }

    public void ResetData()
    {
        health = maxHealth;
        OnMaxHealth?.Invoke(maxHealth);
        OnHealthChanged?.Invoke(health);
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

    [YarnCommand("disableInput")]
    public void DisableInput()
    {
        SetEnableInputs(false);
    }

    [YarnCommand("enableInput")]
    public void EnableInput()
    {
        SetEnableInputs(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Get Inputs
        if (inputEnabled)
        {
            
            //slowModeActivated = slowMode.action.IsPressed();
            //Vector2 moveDir = GetVelocityFromInput();

            if (shoot.action.WasPressedThisFrame())
            {
                Fire();
            }

            //Vector2 vel = moveDir * baseSpeed * SpeedMult;
            //rb.velocity = vel;
        } else
        {
            //rb.velocity = Vector2.zero;
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

        if (move.action.IsInProgress())
        {
            moveInput = move.action.ReadValue<Vector2>().normalized;
        }
        else
        {
            moveInput = Vector2.zero;
        }

        return moveInput;
    }

    public void Damage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);

        OnHealthChanged.Invoke(health);
        if (health <= 0)
        {
            OnHealthDepleted();
        }
    }

    public void OnHealthDepleted()
    {

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
        GameObject newBulletObj = GameObject.Instantiate(bulletPrefab, bulletSpawnPos.position, Quaternion.identity);
        Bullet bullet = newBulletObj.GetComponent<Bullet>();
        bullet.SetVelocity(Vector2.up * bulletSpeed);

        //if (fireTimer <= 0.0f)
        //{
        //    //Fire a new bullet
        //    fireTimer = 1f / fireRate;
        //}
        //else
        //{
        //    fireTimer -= Time.deltaTime;
        //}
    }
}
