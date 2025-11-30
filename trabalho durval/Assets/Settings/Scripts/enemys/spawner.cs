using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;

    [Header("Prefabs")]
    public ShadowEnemyNormal normalPrefab;
    public ShadowEnemyRapido rapidoPrefab;
    public ShadowEnemyObservadora observadoraPrefab;

    [Header("Spawn settings")]
    public float spawnDistanceAhead = 10f;
    public float spawnInterval = 3f;

    private float spawnTimer = 0f;

    void Update()
    {
        if (player == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemies();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemies()
    {
        Vector3 spawnPos = player.position + new Vector3(spawnDistanceAhead, 0f, 0f);

        // Spawn normal
        if (normalPrefab != null)
            Instantiate(normalPrefab, spawnPos, Quaternion.identity);

        // Spawn rápido
        if (rapidoPrefab != null)
            Instantiate(rapidoPrefab, spawnPos, Quaternion.identity);

        // Spawn observadora
        if (observadoraPrefab != null)
            Instantiate(observadoraPrefab, spawnPos, Quaternion.identity);
    }
}
