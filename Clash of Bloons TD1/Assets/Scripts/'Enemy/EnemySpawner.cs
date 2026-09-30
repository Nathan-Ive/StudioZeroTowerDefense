using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject enemyPrefab;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public Transform spawnPoint;

    [Header("Waypoints")]
    public Transform[] waypoints;

    private int nextWaveIndex = 0;
    private float timeBetweenWaves = 5f;
    private float countdown;
    private bool waveIsSpawning = false;

    void Start()
    {
        countdown = timeBetweenWaves;
    }

    void Update()
    {
        if (waveIsSpawning)
            return;

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        else
        {
            countdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnWave()
    {
        waveIsSpawning = true;
        Wave currentWave = waves[nextWaveIndex];

        for (int i = 0; i < currentWave.count; i++)
        {
            SpawnEnemy(currentWave.enemyPrefab);
            yield return new WaitForSeconds(1f / currentWave.rate);
        }

        nextWaveIndex++;
        if (nextWaveIndex >= waves.Length)
        {
            nextWaveIndex = 0;
            Debug.Log("All waves done.");
        }

        waveIsSpawning = false;
    }

    void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        EnemyMovement movement = newEnemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.waypoints = waypoints;
        }
    }
}