using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 3f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 3f, spawnInterval);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}

