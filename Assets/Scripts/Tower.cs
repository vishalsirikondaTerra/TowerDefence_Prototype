using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [Header("Private")]
    public float timer;
    [Space, Header("Self")]
    public const float maxHealth = 100;
    public float health = 100;
    public bool canShoot;

    [Space, Header("Bullet Parameters")]
    public TowerBullet bulletPrefab;
    public float bulletSpeed;
    public float bulletDamage;
    public float fireInterval;
    public int fireCount;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= fireInterval)
        {
            timer = 0;
            FireInTheHole();
        }
        timer += Time.deltaTime;
    }
    void FireInTheHole()
    {
        var tb = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        tb.Fire(bulletDamage, bulletSpeed);
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
}
