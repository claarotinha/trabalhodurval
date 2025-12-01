using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    // -----------------------------
    // Tipos de inimigos
    // -----------------------------
    public enum EnemyType { Normal, Rapida, Observadora }
    public EnemyType enemyType;

    [HideInInspector] public Transform player;

    // -----------------------------
    // Movimento
    // -----------------------------
    [Header("Movimento")]
    [SerializeField] protected float moveSpeed = 2f;            // Velocidade base
    [SerializeField] protected float minDistanceToPlayer = 1.5f; // Distância mínima para não grudar no player

    // -----------------------------
    // Ataque
    // -----------------------------
    [Header("Ataque")]
    public int hitsToPlayer = 2;           // Quantos hits o inimigo dá antes de morrer
    public float despawnDistance = 10f;    // Distância máxima do player para sumir

    // -----------------------------
    // Métodos básicos
    // -----------------------------
    protected virtual void Awake()
    {
        // Encontrar o player na cena (opcional)
        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Movimento suave em direção ao player
    protected void MoveTowardsPlayer(Rigidbody2D rb)
    {
        if (player == null) return;

        float dir = (transform.position.x > player.position.x) ? -1f : 1f;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > minDistanceToPlayer)
        {
            Vector2 targetPos = rb.position + Vector2.right * dir * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPos);
        }
    }

    // Checa se inimigo deve despawnar
    protected bool ShouldDespawn()
    {
        if (player == null) return false;
        return Vector2.Distance(transform.position, player.position) > despawnDistance;
    }

    // Lógica de dano ao player
    protected virtual void AttackPlayer(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(1);

        hitsToPlayer--;
        if (hitsToPlayer <= 0)
            Destroy(gameObject);
    }

    // Trigger padrão
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        AttackPlayer(collision);
    }
}
