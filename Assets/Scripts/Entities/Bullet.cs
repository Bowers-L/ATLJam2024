using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isFromPlayer;
    [SerializeField] private int damage;

    [SerializeField] private SpriteRenderer fill;

    private Rigidbody2D rb;
    private float acceleration;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    public void SetVelocity(Vector2 vel)
    {
        rb.velocity = vel;
    }

    public void SetAcceleration(float accel)
    {
        acceleration = accel;
    }

    private void Update()
    {
        if (transform.position.x > 5.5f || transform.position.x < -5.5f || transform.position.y > 5.5f || transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude + acceleration * Time.deltaTime);
    }

    public void SetColor(Color color)
    {
        fill.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponentInParent<Bullet>() != null)
        {
            //Don't do anything on bullet-bullet collisions.
            return;
        }

        Debug.Log($"Bullet Collided: {collider}");

        PlayerController player = collider.GetComponentInParent<PlayerController>();
        EnemyController enemy = collider.GetComponentInParent<EnemyController>();
        if (isFromPlayer)
        {
            if (player != null)
            {
                return;
            }

            if (enemy != null)
            {
                enemy.Damage(damage);
            }
            Destroy(this.gameObject);
        } else
        {
            if (enemy != null)
            {
                return;
            }

            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
