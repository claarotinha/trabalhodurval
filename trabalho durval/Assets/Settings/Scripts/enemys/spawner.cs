using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyProbability
    {
        public GameObject prefab;
        [Range(0, 100)]
        public float probability = 50f;
    }

    [Header("Inimigos e Probabilidades")]
    public EnemyProbability[] enemies;

    [Header("Configurações de Spawn")]
    public float minDistance = 3f;
    public float maxDistance = 5f;
    public float spawnInterval = 2f;
    public int maxEnemies = 5;
    public LayerMask groundLayer;
    public float spawnHeight = 0f;

    // 🔒 FLAG GLOBAL DA ZONA SEGURA
    public static bool inZonaSegura = false;

    private Transform player;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player não encontrado! Coloque a tag 'Player'.");
            return;
        }

        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (player == null) return;

        // 🚫 NÃO SPAWNA SE ESTIVER EM ZONA SEGURA
        if (inZonaSegura) return;

        spawnedEnemies.RemoveAll(e => e == null);
        if (spawnedEnemies.Count >= maxEnemies) return;

        GameObject prefab = ChoosePrefab();
        if (prefab == null) return;

        Vector2 spawnPos = GetGroundPosition(prefab);
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        spawnedEnemies.Add(enemy);
    }

    GameObject ChoosePrefab()
    {
        if (enemies.Length == 0) return null;

        float total = 0f;
        foreach (var e in enemies)
            total += e.probability;

        float rand = Random.Range(0f, total);
        float current = 0f;

        foreach (var e in enemies)
        {
            current += e.probability;
            if (rand <= current)
                return e.prefab;
        }

        return enemies[0].prefab;
    }

    Vector2 GetGroundPosition(GameObject prefab)
    {
        EnemyBase enemyBase = prefab.GetComponent<EnemyBase>();
        float distance = Random.Range(minDistance, maxDistance);

        // Observadora nasce à frente, outras atrás
        float xPos = (enemyBase != null && enemyBase.enemyType == EnemyBase.EnemyType.Observadora)
            ? player.position.x + distance
            : player.position.x - distance;

        float rayStartY = player.position.y + 20f;
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(xPos, rayStartY),
            Vector2.down,
            50f,
            groundLayer
        );

        float yOffset = 0.5f;
        Collider2D col = prefab.GetComponent<Collider2D>();
        if (col != null)
            yOffset = col.bounds.extents.y;

        if (hit.collider != null)
            return new Vector2(xPos, hit.point.y + yOffset + spawnHeight);

        // fallback
        return new Vector2(xPos, player.position.y + spawnHeight + yOffset);
    }
}
