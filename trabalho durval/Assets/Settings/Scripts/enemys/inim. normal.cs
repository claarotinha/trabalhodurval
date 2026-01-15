using UnityEngine;

public class EntidadeNormal : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();

        enemyType = EnemyType.Normal;
        moveSpeed = 2.5f;
        reactionDelay = 0.7f;
        despawnDistance = 12f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        collision.GetComponent<PlayerHealth>()?.TakeDamage(1);
        Destroy(gameObject);
    }
}
