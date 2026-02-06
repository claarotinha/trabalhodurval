using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 2f;
    public float distance = 3f;

    [Header("Dano")]
    public int damage = 1;

    private Vector3 startPoint;
    private int direction = 1;

    void Start()
    {
        startPoint = transform.position;
    }

    void Update()
    {
        // Move para direita e esquerda a partir do ponto inicial
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Se passar da distância permitida, inverte
        if (Mathf.Abs(transform.position.x - startPoint.x) >= distance)
        {
            direction *= -1;
            Flip();
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damage, transform);
            }
        }
    }
}
