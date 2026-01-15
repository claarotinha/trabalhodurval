using UnityEngine;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour
{
    public enum EnemyType { Normal, Rapida, Observadora }
    public EnemyType enemyType;

    [HideInInspector] public Transform player;
    protected Rigidbody2D rb;

    [Header("Movimento")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float minDistanceToPlayer = 0.5f;
    [SerializeField] protected float reactionDelay = 0.5f;

    [Header("Despawn")]
    [SerializeField] protected float despawnDistance = 12f;

    protected bool canMove = false;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.isKinematic = false;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;

        StartCoroutine(ReactionDelay());
    }

    IEnumerator ReactionDelay()
    {
        yield return new WaitForSeconds(reactionDelay);
        canMove = true;
    }

    protected virtual void FixedUpdate()
    {
        if (!canMove || player == null) return;

        MoveTowardsPlayer(moveSpeed);

        if (Vector2.Distance(transform.position, player.position) > despawnDistance)
            Destroy(gameObject);
    }

    protected void MoveTowardsPlayer(float speed)
    {
        float distance = Vector2.Distance(rb.position, player.position);
        if (distance <= minDistanceToPlayer) return;

        float dir = Mathf.Sign(player.position.x - rb.position.x);
        rb.MovePosition(rb.position + Vector2.right * dir * speed * Time.fixedDeltaTime);
    }
}
