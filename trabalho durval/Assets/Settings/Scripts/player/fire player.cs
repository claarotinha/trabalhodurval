using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 2f;
    public int damage = 1;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Ignora colisão com o player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Physics2D.IgnoreCollision(
                GetComponent<Collider2D>(),
                player.GetComponent<Collider2D>()
            );
        }

        Destroy(gameObject, lifeTime);
    }

    // Disparo real (impulso)
    public void Shoot(float direction)
    {
        rb.AddForce(Vector2.right * direction * speed, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
