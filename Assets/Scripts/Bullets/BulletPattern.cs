using UnityEngine;

[CreateAssetMenu(fileName = "bulletPattern", menuName = "CUSTOM/Bullet Pattern")]
public class BulletPattern : ScriptableObject
{
    public int numBullets = 1;
    public float startRadialOffset = 0.0f;
    public float startAngularOffset;    //0.0 = start on right
    public float arcSpread;
    public float fireRate = 1.0f;
    public float spinPerFire = 0.0f;
    public float bulletSpeed = 1.0f;
    public float bulletAccel = 2.0f;
    public GameObject bulletPrefab;
    public Color color = Color.white;
}
