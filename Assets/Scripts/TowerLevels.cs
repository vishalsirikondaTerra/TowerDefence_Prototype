using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLevels : MonoBehaviour
{
    public TowerLevelStat[] levelStats;


    public TowerLevelStat GetLevelStatAt(int level)
    {
        if (level >= levelStats.Length)
        {
            level = levelStats.Length - 1;
        }
        if (level < 0)
        {
            level = 0;
        }
        return levelStats[level - 1];
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
