using UnityEngine;

public class EntidadeNormal : EnemyBase
{
    private Rigidbody2D rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.isKinematic = true;
        rb.gravityScale = 0;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;

        // Configura valores específicos do tipo Normal
        enemyType = EnemyType.Normal;
        moveSpeed = 3f;
        minDistanceToPlayer = 0.5f;
        despawnDistance = 12f;
        hitsToPlayer = 1;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

        float distance = Vector2.Distance((Vector2)player.position, rb.position);

        if (distance <= minDistanceToPlayer)
        {
            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(1);

            Destroy(gameObject);
        }

        if (distance > despawnDistance)
            Destroy(gameObject);
    }
}
