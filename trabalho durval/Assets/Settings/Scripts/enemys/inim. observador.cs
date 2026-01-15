using UnityEngine;

public class Observadora : EnemyBase
{
    [Header("Efeito Psicológico")]
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private int damage = 1;

    private bool effectApplied;

    protected override void Awake()
    {
        base.Awake();

        enemyType = EnemyType.Observadora;
        moveSpeed = 0f;
        reactionDelay = 0f;
        despawnDistance = 8f;
    }

    protected override void FixedUpdate() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (effectApplied) return;
        if (!collision.CompareTag("Player")) return;

        effectApplied = true;

        collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);

        PlayerMovement2D_TagBased move =
            collision.GetComponent<PlayerMovement2D_TagBased>();

        if (move != null)
            move.ApplySlow(slowMultiplier, slowDuration);

        Destroy(gameObject);
    }
}
