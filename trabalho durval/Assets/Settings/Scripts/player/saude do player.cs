using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")]
    public Slider lifeBar;

    [Header("Knockback")]
    public float knockbackForce = 8f;
    public float knockbackUpForce = 3f;
    public float stunDuration = 0.15f;

    private Rigidbody2D rb;
    private bool isStunned;
    private Vector3 respawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        respawnPoint = transform.position;

        if (lifeBar != null)
        {
            lifeBar.maxValue = maxHealth;
            lifeBar.value = currentHealth;
        }
    }

    public void SetCheckpoint(Vector3 point)
    {
        respawnPoint = point;
    }

    public void TakeDamage(int amount, Transform enemy = null)
    {
        if (isStunned) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        UpdateUI();

        if (enemy != null)
            ApplyKnockback(enemy);

        if (currentHealth <= 0)
            Die();
    }

    void ApplyKnockback(Transform enemy)
    {
        isStunned = true;

        float dir = Mathf.Sign(transform.position.x - enemy.position.x);
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(dir * knockbackForce, knockbackUpForce), ForceMode2D.Impulse);

        Invoke(nameof(ResetStun), stunDuration);
    }

    void ResetStun()
    {
        isStunned = false;
    }

    void Die()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = respawnPoint;

        PlayerMovement2D_TagBased move = GetComponent<PlayerMovement2D_TagBased>();
        if (move != null)
            move.IgnoreCameraLimit(0.15f);

        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.ResetCameraTo(respawnPoint);

        currentHealth = maxHealth;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (lifeBar != null)
            lifeBar.value = currentHealth;
    }
}
