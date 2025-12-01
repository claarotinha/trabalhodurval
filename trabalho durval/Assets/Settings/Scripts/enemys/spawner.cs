using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnerFinal : MonoBehaviour
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

    private Transform player;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player não encontrado! Coloque tag 'Player'.");
            return;
        }

        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (player == null) return;

        spawnedEnemies.RemoveAll(e => e == null);
        if (spawnedEnemies.Count >= maxEnemies) return;

        GameObject prefab = ChoosePrefab();
        if (prefab == null) return;

        Vector2 spawnPos = GetGroundPosition(prefab);
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Configura para que entidades normais e rápidas causem dano e sumam
        EnemyBase enemyScript = enemy.GetComponent<EnemyBase>();
        if (enemyScript != null && (enemyScript.enemyType == EnemyBase.EnemyType.Normal || enemyScript.enemyType == EnemyBase.EnemyType.Rapida))
        {
            EnemyCollisionDamage collision = enemy.AddComponent<EnemyCollisionDamage>();
            collision.player = player;
            collision.damage = 1;
        }

        spawnedEnemies.Add(enemy);
    }

    GameObject ChoosePrefab()
    {
        if (enemies.Length == 0) return null;

        float total = 0f;
        foreach (var e in enemies) total += e.probability;

        float rand = Random.Range(0f, total);
        float current = 0f;

        foreach (var e in enemies)
        {
            current += e.probability;
            if (rand <= current) return e.prefab;
        }

        return enemies[0].prefab;
    }

    Vector2 GetGroundPosition(GameObject prefab)
    {
        EnemyBase enemyBase = prefab.GetComponent<EnemyBase>();
        float distance = Random.Range(minDistance, maxDistance);
        float xPos = (enemyBase.enemyType == EnemyBase.EnemyType.Observadora)
                     ? player.position.x + distance
                     : player.position.x - distance;

        // Raycast do alto até o chão
        float rayStartY = player.position.y + 20f;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(xPos, rayStartY), Vector2.down, 50f, groundLayer);

        float yOffset = 0.5f;
        Collider2D col = prefab.GetComponent<Collider2D>();
        if (col != null) yOffset = col.bounds.extents.y;

        if (hit.collider != null)
            return new Vector2(xPos, hit.point.y + yOffset + spawnHeight);

        // fallback
        return new Vector2(xPos, player.position.y + spawnHeight + yOffset);
    }

    void Update()
    {
        // Debug gizmos para área de spawn
        if (player == null) return;

        Debug.DrawLine(new Vector2(player.position.x - maxDistance, player.position.y - 10), 
                       new Vector2(player.position.x - maxDistance, player.position.y + 10), Color.yellow);

        Debug.DrawLine(new Vector2(player.position.x + maxDistance, player.position.y - 10), 
                       new Vector2(player.position.x + maxDistance, player.position.y + 10), Color.yellow);
    }
}

// Script simples para dano e destruição ao colidir
public class EnemyCollisionDamage : MonoBehaviour
{
    public Transform player;
    public int damage = 1;
    private bool hasHit = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (hasHit) return;
        if (col.transform != player) return;

        // Aqui você aplica dano ao player (supondo que o player tenha método TakeDamage)
        var playerHealth = player.GetComponent<PlayerHealth>(); 
        if (playerHealth != null) playerHealth.TakeDamage(damage);

        hasHit = true;
        Destroy(gameObject);
    }
}
