using UnityEngine;

public class BossStompArea : MonoBehaviour
{
    public int damage = 1;
    public float duration = 0.5f;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
