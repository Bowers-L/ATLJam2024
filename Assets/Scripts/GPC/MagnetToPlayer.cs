using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetToPlayer : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;

    private PlayerController playerAttractedTo;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        playerAttractedTo = null;
    }

    private void Update()
    {
        if (playerAttractedTo != null)
        {
            Vector2 orbToPlayer = (playerAttractedTo.transform.position - transform.position).normalized;
            Vector2 dv = orbToPlayer * acceleration * Time.deltaTime;
            rb.velocity += dv; 
            rb.velocity = Mathf.Clamp(rb.velocity.magnitude, 0.0f, maxSpeed) * rb.velocity.normalized;

            //Debug.Log($"deltaVel: {dv}");
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            playerAttractedTo = player;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            playerAttractedTo = player;
        }
    }
}
