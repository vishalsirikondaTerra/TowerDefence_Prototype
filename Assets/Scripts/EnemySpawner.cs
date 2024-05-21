using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnParent;

    public Transform[] spawnLanes;
    public float timer;
    private int lastLane;

    [Space, Header("Parameters")]
    public Enemy enemyToSpawn;
    public Wave[] waves;
    public Wave currentWave;

    public void Awake()
    {
        spawnLanes = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            spawnLanes[i] = spawnParent.GetChild(i);
        }
    }
    void Start()
    {
        currentWave = waves[0];
        lastLane = -1;
    }
    public void Update()
    {
        if (timer >= currentWave.spawnInterval)
        {
            timer = 0;
            SpawnEnemies();
        }
        timer += Time.deltaTime;
    }

    private void SpawnEnemies()
    {
        int enemies = currentWave.totalEnemiesPerWave;
        for (int i = 0; i < enemies; i++)
        {
            int lane = -1;
            while (lane == -1 || lane == lastLane)
            {
                lane = Random.Range(0, 4);
            }
            lastLane = lane;
            Transform laneTr = spawnLanes[lane];
            var enemy = Instantiate(enemyToSpawn, laneTr.position, laneTr.localRotation);
            enemy.Spawn(1);

        }

    }

    [Serializable]
    public struct Wave
    {
        public float spawnInterval;
        public int totalEnemiesPerWave;

    }
}
