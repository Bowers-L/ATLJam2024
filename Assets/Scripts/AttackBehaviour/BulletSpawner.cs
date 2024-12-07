using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //[SerializeField] List<BulletPatternArc> patternDeck;

    //public class BulletPatternInstance {
    //    public BulletPatternArc pattern;
    //    public int fireCount;
    //    public float fireTimer;
    //    public float destroyTimer;
    //    public bool isActive;

    //    public BulletPatternInstance(BulletPatternArc pattern, float destroyAfter = Mathf.Infinity)
    //    {
    //        this.pattern = pattern;
    //        this.fireTimer = 0.0f;
    //        this.destroyTimer = destroyAfter;
    //    }
    //}



    //private List<BulletPatternInstance> activePatterns;

    //private List<Bullet> activeBullets;

    ////
    //// Start is called before the first frame update
    //void Start()
    //{
    //    activePatterns = new List<BulletPatternInstance>();
    //    activeBullets = new List<Bullet>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (activePatterns.Count > 0)
    //    {
    //        foreach (BulletPatternInstance bullets in activePatterns)
    //        {
    //            UpdateBullets(bullets);
    //        }
    //    }
    //}

    //public void StartPattern(int index, float destroyAfter = Mathf.Infinity)
    //{
    //    BulletPatternInstance bullets = new BulletPatternInstance(patternDeck[index], destroyAfter);
    //    bullets.fireTimer = 0.0f;
    //    activePatterns.Add(bullets);
    //}

    //private void UpdateBullets(BulletPatternInstance instance)
    //{
    //    float angleStep = instance.pattern.arcSpread;

    //    if (instance.fireTimer <= 0.0f)
    //    {
    //        for (int i = 0; i < instance.pattern.numBullets; i++)
    //        {
    //            Vector2 radialDir = instance.pattern.startOffset;
    //            //float angle = angleStep * (i - instance.pattern.numBullets / 2) + instance.pattern.startOffset;
    //            //float dirX = Mathf.Cos(angle * Mathf.Deg2Rad + instance.fireCount * instance.pattern.spinPerFire);
    //            //float dirY = Mathf.Sin(angle * Mathf.Deg2Rad + instance.fireCount * instance.pattern.spinPerFire);

    //            //Vector2 velDir = new Vector2(dirX, dirY);
    //            Vector2 velDir = Vector2.down;
    //            Vector2 spawnPos = (Vector2)transform.position + instance.pattern.startOffset;

    //            GameObject newBulletObj = GameObject.Instantiate(instance.pattern.bulletPrefab, spawnPos, Quaternion.identity);
    //            Bullet bullet = newBulletObj.GetComponent<Bullet>();
    //            SpriteRenderer sr = newBulletObj.GetComponent<SpriteRenderer>();
    //            sr.color = instance.pattern.color;
    //            bullet.SetVelocity(velDir * instance.pattern.bulletSpeed);
    //            bullet.SetAcceleration(instance.pattern.bulletAccel);
    //        }

    //        instance.fireCount++;
    //        instance.fireTimer = 1f / instance.pattern.fireRate;
    //    } else
    //    {
    //        instance.fireTimer -= Time.deltaTime;
    //    }

    //    instance.destroyTimer -= Time.deltaTime;
    //    if (instance.destroyTimer < 0.0f)
    //    {
    //        activePatterns.Remove(instance);    //Should be garbage collected.
    //    }
    //}

    //public void StopAllPatterns()
    //{
    //    activePatterns.Clear();
    //}

    //public void Play()
    //{
    //    playing = true;
    //}

    //public void Stop()
    //{
    //    playing = false;
    //}
}
