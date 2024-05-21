using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevels : MonoBehaviour
{
    public EnemyLevelStat[] levelStats;


    public EnemyLevelStat GetLevelStatAt(int level)
    {
        level = level - 1;
        if (level >= levelStats.Length)
        {
            level = levelStats.Length - 1;
        }
        if (level < 0)
        {
            level = 0;
        }
        return levelStats[level];
    }
}
[System.Serializable]
public struct EnemyLevelStat
{
    public float moveSpeed;
    public float damage;
    public float maxHealth;
    public float attackEvery;

}