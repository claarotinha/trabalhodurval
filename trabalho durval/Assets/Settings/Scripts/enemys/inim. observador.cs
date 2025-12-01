using UnityEngine;

public class Observadora : EnemyBase
{
    private Rigidbody2D rb;

    [Header("Configuração")]
    [SerializeField] private float lifeTimeIfNotTouched = 3f; // tempo antes de auto-destruir se não tocar no player
    private float timer = 0f;
    private bool touchedPlayer = false;

    protected override void Awake()
    {
        base.Awake();

        enemyType = EnemyType.Observadora;
        hitsToPlayer = 1;
        despawnDistance = 10f;

        moveSpeed = 0f;
        minDistanceToPlayer = 0f;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.isKinematic = true;
        rb.gravityScale = 0;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;

        timer = lifeTimeIfNotTouched;
    }

    private void Update()
    {
        if (touchedPlayer) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Destroy(gameObject); // destrói se o player não atravessou
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        touchedPlayer = true; // player tocou antes do timer
        var playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(1);

        Destroy(gameObject);
    }
}
