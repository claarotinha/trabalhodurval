using UnityEngine;

public class FallingPlatformRespawn : MonoBehaviour
{
    [Header("Configuração")]
    public float fallDelay = 0.5f;       // tempo antes de começar a cair
    public float respawnDelay = 3f;      // tempo para voltar ao normal

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private Quaternion initialRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic; // começa fixa
        rb.gravityScale = 0f;

        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Invoke(nameof(Fall), fallDelay);
        }
    }

    void Fall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;

        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        // reseta posição e estado
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
