using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public const float maxHealth = 100;
    public float moveSpeed;
    public float damage;
    public float health;

    public bool canMove;

    public float attackEvery;
    public float attackTimer;

    public Transform towerAt;
    public Ray ray;
    public float towerAtDistance = 3f;
    public LayerMask towerMask;
    public Tower foundTower;


    void Start()
    {
        health = maxHealth;
        ray = new Ray(transform.position, transform.forward * towerAtDistance);
    }
    public void OnTriggerEnter(Collider collider)
    {
        var bullet = collider.GetComponent<TowerBullet>();
        if (bullet != null)
        {
            bullet.TurnOff();
            Damage(bullet.damageValue);
        }
    }
    void Update()
    {
        if (canMove)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        if (Physics.SphereCast(ray, 0.3f, out RaycastHit hit, 1f, towerMask))
        {
            canMove = false;
            if (attackTimer >= attackEvery)
            {
                foundTower = hit.collider.GetComponent<Tower>();
                attackTimer = 0;
                foundTower.Hit(damage);
            }

            attackTimer += Time.deltaTime;
        }
        else
        {
            canMove = true;
        }
    }

    public void Damage(float val)
    {
        health -= val;
        if (health <= 0)
        {
            gameObject.SetActive(false);
            $"{gameObject.name} Killed".LOG();
        }
    }

    internal void Spawn()
    {
        gameObject.SetActive(true);
        canMove = true;
    }

    void OnDrawGizmos()
    {
        ray = new Ray(transform.position, transform.forward * towerAtDistance);
        Gizmos.DrawRay(ray.origin, ray.direction * towerAtDistance);
        Gizmos.DrawSphere((ray.direction * towerAtDistance) + transform.position, 0.5f);
    }
}
