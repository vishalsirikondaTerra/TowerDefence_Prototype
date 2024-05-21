using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [Header("Private")]
    public float timer;
    [Space, Header("Self")]
    public bool canShoot;
    public float health = 100;

    [Space, Header("Bullet Parameters")]
    public TowerBullet bulletPrefab;

    public TowerLevels towerLevels;
    public TowerLevelStat currentStat;

    public int currentLevel = 1;

    void Awake()
    {
        currentStat = towerLevels.GetLevelStatAt(1);
        health = currentStat.maxHealth;

    }
    void Start()
    {
        canShoot = true;
    }

    void Update()
    {
        if (!canShoot)
        {
            return;
        }
        if (timer >= currentStat.fireInterval)
        {
            timer = 0;
            FireInTheHole();
        }
        timer += Time.deltaTime;
    }
    void FireInTheHole()
    {
        var tb = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        tb.Fire(currentStat.bulletDamage, currentStat.bulletSpeed, transform.position);
    }

    internal void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
            $"{gameObject.name} Killed".LOG();
        }
    }

    public void LevelIncrease()
    {
        currentLevel += 1;
        currentStat = towerLevels.GetLevelStatAt(currentLevel);
        health = currentStat.maxHealth;
        timer = 0;
    }

}
