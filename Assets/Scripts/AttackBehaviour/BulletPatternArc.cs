using UnityEngine;

[CreateAssetMenu(fileName = "bulletPattern", menuName = "CUSTOM/Bullet Pattern")]
public class BulletPatternArc : ScriptableObject
{
    [Header("Spawn Position")]
    public int numBullets;
    public Vector2 startOffset;
    public float distanceBetween;
    public float angleBetween;

    [Header("Movement")]
    //public float fireRate = 1.0f;
    //public float spinPerFire = 0.0f;
    public float bulletSpeed = 1.0f;
    public float bulletAccel = 2.0f;

    [Header("Visuals")]
    public GameObject bulletPrefab;
    public Color color = Color.white;
}
