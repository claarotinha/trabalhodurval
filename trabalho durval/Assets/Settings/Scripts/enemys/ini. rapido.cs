using UnityEngine;

public class EntidadeRapida : EnemyBase
{
    private PlayerMovement2D_TagBased playerMove;

    protected override void Awake()
    {
        base.Awake();

        enemyType = EnemyType.Rapida;
        moveSpeed = 3f;
        reactionDelay = 0.3f;
        despawnDistance = 14f;

        if (player != null)
            playerMove = player.GetComponent<PlayerMovement2D_TagBased>();
    }

    protected override void FixedUpdate()
    {
        if (!canMove || player == null) return;

        float speed = moveSpeed;

        // efeito do shift (corrida)
        if (playerMove != null && Input.GetKey(KeyCode.LeftShift))
            speed *= 1.5f;

        // 🔹 aplica o slow global
        if (EffectManager.Instance != null)
            speed *= EffectManager.Instance.globalSlowMultiplier;

        MoveTowardsPlayer(speed);

        if (Vector2.Distance(transform.position, player.position) > despawnDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.GetComponent<PlayerHealth>()?.TakeDamage(1, transform);
        Destroy(gameObject);
    }
}
