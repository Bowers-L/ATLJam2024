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

    [SerializeField] private BulletPatternArc bulletPattern;

    private void Start()
    {
        health = maxHealth;
        OnMaxHealth?.Invoke(maxHealth);
        OnHealthChanged?.Invoke(health);

        StartCoroutine(AttackRoutine());
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

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            FireBulletsArc(bulletPattern);
        }
    }

    #region Bullet Spawning
    public void FireBulletsArc(BulletPatternArc bulletData)
    {
        Vector2 spawnPos = (Vector2) transform.position + bulletData.startOffset;
        Vector2 radialDir = bulletData.startOffset.normalized;
        Vector2 stepDir = new Vector2(-radialDir.y, radialDir.x);
        for (int i = 0; i < 1; i++)
        {
            float centeredI = (float) (i - bulletData.numBullets / 2);
            if (bulletData.numBullets % 2 == 0)
            {
                centeredI += 0.5f;
            }

            Vector2 velDir = Vector2.down;

            for (int sign = -1; sign <= 1; sign += 2)
            {
                if (i == 0 && sign == -1)
                {
                    continue;
                }

                float rotateAngle = 90.0f - bulletData.angleBetween;
                GameObject newBulletObj = GameObject.Instantiate(bulletData.bulletPrefab, spawnPos, Quaternion.identity);
                Bullet bullet = newBulletObj.GetComponent<Bullet>();
                bullet.SetColor(bulletData.color);
                bullet.SetVelocity(velDir * bulletData.bulletSpeed);
                bullet.SetAcceleration(bulletData.bulletAccel);
            }

            //Rotate by bulletData.angleBetween ccw.
            float XIter = stepDir.x * Mathf.Cos(bulletData.angleBetween) + stepDir.y * -Mathf.Sin(bulletData.angleBetween);
            float YIter = stepDir.x * Mathf.Sin(bulletData.angleBetween) + stepDir.y * Mathf.Cos(bulletData.angleBetween);
            stepDir += new Vector2(XIter, YIter);
            spawnPos += stepDir * bulletData.distanceBetween;
        }
    }
    #endregion
}
