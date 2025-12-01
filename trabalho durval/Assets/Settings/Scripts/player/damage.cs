using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Valores de Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")]
    public Slider lifeBar;

    [Header("Knockback")]
    public float knockbackForce = 10f;
    public float knockbackUpForce = 4f;
    public float stunDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isStunned = false;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody2D>();

        if (lifeBar != null)
        {
            lifeBar.minValue = 0;
            lifeBar.maxValue = maxHealth;
            lifeBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount, Transform enemyPos = null)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateLifeUI();

        // Aplica knockback se o inimigo foi passado
        if (enemyPos != null)
            ApplyKnockback(enemyPos);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void ApplyKnockback(Transform enemy)
    {
        if (isStunned) return;

        isStunned = true;

        // Direção contrária ao inimigo
        float direction = Mathf.Sign(transform.position.x - enemy.position.x);

        // Zera velocidade antes
        rb.linearVelocity = Vector2.zero;

        // Força para trás + para cima
        Vector2 force = new Vector2(direction * knockbackForce, knockbackUpForce);
        rb.AddForce(force, ForceMode2D.Impulse);

        // Remove stun depois
        Invoke(nameof(ResetStun), stunDuration);
    }

    void ResetStun()
    {
        isStunned = false;
    }

    void UpdateLifeUI()
    {
        if (lifeBar != null)
            lifeBar.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("O Player morreu!");
    }
}
