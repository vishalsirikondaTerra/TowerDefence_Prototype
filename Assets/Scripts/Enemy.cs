using System;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform trigger;
    public bool canMove;

    private float attackTimer;

    public Ray ray;
    public float towerAtDistance = 3f;
    public LayerMask towerMask;
    public EnemyLevelStat currentStat;
    public EnemyLevels enemyLevels;

    public TextMeshProUGUI text;

    public float currentHealth;
    public int currentLevel = 1;
    [Space(20)]
    public Tower foundTower;

    public void Awake()
    {
        currentLevel = 1;
        currentStat = enemyLevels.GetLevelStatAt(1);
        currentHealth = currentStat.maxHealth;
    }
    void Start()
    {
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
            transform.Translate(Vector3.forward * currentStat.moveSpeed * Time.deltaTime);
        }
        ray = new Ray(transform.position, transform.forward * towerAtDistance);
        trigger.position = (ray.direction * towerAtDistance) + transform.position;
        if (Physics.SphereCast(ray, 0.3f, out RaycastHit hit, 1f, towerMask))
        {
            canMove = false;
            if (attackTimer >= currentStat.attackEvery)
            {
                foundTower = hit.collider.GetComponent<Tower>();
                attackTimer = 0;
                foundTower.Hit(currentStat.damage);
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
        currentHealth -= val;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            $"{gameObject.name} Killed".LOG();
        }
    }

    internal void Spawn(int level)
    {
        gameObject.SetActive(true);
        canMove = true;
        currentStat = enemyLevels.GetLevelStatAt(level);
        currentHealth = currentStat.maxHealth;
        currentLevel = level;
        text.text = $"{currentLevel}";
    }

    void OnDrawGizmos()
    {
        ray = new Ray(transform.position, transform.forward * towerAtDistance);
        Gizmos.DrawRay(ray.origin, ray.direction * towerAtDistance);
        Gizmos.DrawSphere((ray.direction * towerAtDistance) + transform.position, 0.5f);
    }
}
