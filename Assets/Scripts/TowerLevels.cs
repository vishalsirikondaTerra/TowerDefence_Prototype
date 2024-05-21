using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevels : MonoBehaviour
{
    public TowerLevelStat[] levelStats;


    public TowerLevelStat GetLevelStatAt(int level)
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
public struct TowerLevelStat
{
    public float maxHealth;

    public float bulletSpeed;
    public float bulletDamage;
    public float fireInterval;
    public int fireCount;
}
