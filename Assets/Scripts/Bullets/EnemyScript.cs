using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "enemyScript", menuName = "CUSTOM/Enemy Script")]
public class EnemyScript : ScriptableObject
{
    public List<EnemyAction> actions;
    public bool despawnAfterFinish;
}

[System.Serializable]
public class EnemyAction
{
    public float timeSinceLast;
    public Vector2 translate;
    public float translateDuration;
    public int bulletPatternIndex = -1;
    public float startBulletPatternAtPercent;  //Value from 0-1, 0 = start of move, 1 = end of move
}