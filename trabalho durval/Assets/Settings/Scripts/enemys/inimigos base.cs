using UnityEngine;

public class ShadowEnemyBase : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 2f;

    [Header("Distância e dissolução")]
    public float maxDistanceToPlayer = 15f;
    public float dissolveSpeed = 0.05f;           
    public float dissolveDelayAfterDistance = 2f; 

    [Header("Spawn buffer")]
    public float spawnBufferTime = 1f; 

    [Header("Ground Check")]
    public LayerMask walkableLayer;
    public float groundCheckDistance = 1f;

    protected Transform player;
    protected SpriteRenderer sr;
    protected bool isDissolving = false;
    protected float distanceTimer = 0f;
    protected float spawnBufferCounter;

    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player")?.transform;
        spawnBufferCounter = spawnBufferTime;

        if (sr == null)
            Debug.LogError("SpriteRenderer não encontrado!");
        if (player == null)
            Debug.LogError("Player não encontrado na cena!");
    }

    protected virtual void Update()
    {
        if (player == null) return;

        Move();
        CheckGround();

        // durante buffer inicial, não dissolve
        if (spawnBufferCounter > 0f)
        {
            spawnBufferCounter -= Time.deltaTime;
            return;
        }

        CheckDistance();
        Dissolve();
    }

    protected virtual void Move()
    {
        if (player == null) return;

        // Normais e rápidas perseguem o player
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += new Vector3(dir.x, 0, 0) * speed * Time.deltaTime;
    }

    protected virtual void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, walkableLayer);
        if (hit.collider == null)
            Die(); 
    }

    protected virtual void CheckDistance()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > maxDistanceToPlayer)
        {
            distanceTimer += Time.deltaTime;
            if (distanceTimer >= dissolveDelayAfterDistance)
                isDissolving = true;
        }
        else
        {
            distanceTimer = 0f;
            isDissolving = false;
        }
    }

    protected virtual void Dissolve()
    {
        if (!isDissolving || sr == null) return;

        Color c = sr.color;
        c.a -= Time.deltaTime * dissolveSpeed;
        sr.color = c;

        if (c.a <= 0f)
            Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
