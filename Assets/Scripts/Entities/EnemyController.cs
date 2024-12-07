using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 20;

    private int health;

    public UnityEvent<int> OnMaxHealth;
    public UnityEvent<int> OnHealthChanged;

    private void Start()
    {
        health = maxHealth;
        OnMaxHealth?.Invoke(maxHealth);
        OnHealthChanged?.Invoke(health);
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
}
