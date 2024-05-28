using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnParent;

    public Transform[] spawnLanes;
    public float timer;
    public float waveintervaldelay;

    private int lastLane;

    [Space, Header("Parameters")]
    public Enemy enemyToSpawn;
    public Wave[] waves;
    public Wave currentWave;
    public bool spawningInProgress;
    [SerializeField] private int currentWaveIndex;
    [SerializeField] private GameManager gameManager;
    public bool changeWave;


    public void Awake()
    {
        spawnLanes = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            spawnLanes[i] = spawnParent.GetChild(i);
        }
        gameManager = GetComponent<GameManager>();
    }
    void Start()
    {
        currentWaveIndex = 0;
        currentWave = waves[currentWaveIndex];
        lastLane = -1;
    }
    public void Update()
    {
        if (spawningInProgress)
        {
            return;
        }
        if (!changeWave)
        {

            if (timer >= currentWave.spawnInterval)
            {
                timer = 0;
                StartCoroutine(SpawnEnemies());
            }
        }
        else
        {
            if(timer >= waveintervaldelay)
            {
                changeWave = false;
            }
        }

        timer += Time.deltaTime;
    }

    private IEnumerator SpawnEnemies()
    {
        spawningInProgress = true;
        int enemies = currentWave.GetSpawnCount();
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
            enemy.Spawn(currentWave.GetRandomLevel());
            yield return new WaitForSeconds(Random.Range(0.2f, 0.5f));
        }
        currentWave.totalEnemiesPerWave -= enemies;
        if (currentWave.totalEnemiesPerWave <= 0)
        {
            NextWave();
        }
        spawningInProgress = false;
    }
    private void NextWave()
    {
        currentWaveIndex += 1;
        changeWave = true;
        gameManager.NextWave();
        $" Highest Level Merge = {gameManager.highestLevel}".LOG();
        if (currentWaveIndex >= waves.Length)
        {
            currentWaveIndex = waves.Length - 1;
        }
        currentWave = waves[currentWaveIndex];
    }

    [Serializable]
    public struct Wave
    {
        public float spawnInterval;
        public Vector2Int spawnRandomRange;
        public int totalEnemiesPerWave;
        public int[] spawnEnemyOfLevels;

        public int GetSpawnCount()
        {
            return Random.Range(spawnRandomRange.x, spawnRandomRange.y + 1);
        }
        public int GetRandomLevel()
        {
            return spawnEnemyOfLevels[Random.Range(0, spawnEnemyOfLevels.Length)];
        }

    }

}
