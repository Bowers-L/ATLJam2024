using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private bool isFromPlayer;  //I'm too lazy to make entirely separate script lol.

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
        rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude + acceleration * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponentInParent<Bullet>() != null)
        {
            //Don't do anything on bullet-bullet collisions.
            return;
        }

        PlayerController player = collision.collider.GetComponentInParent<PlayerController>();
        //Enemy enemy = collision.collider.GetComponentInParent<Enemy>();
        if (isFromPlayer)
        {
            if (player != null)
            {
                return;
            }

            //if (enemy != null)
            //{
            //    enemy.Damage();
            //}
            Destroy(this.gameObject);
        } else
        {
            //if (enemy != null)
            //{
            //    return;
            //}

            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
