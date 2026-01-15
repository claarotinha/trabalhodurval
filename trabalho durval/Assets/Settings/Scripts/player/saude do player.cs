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
    public float knockbackForce = 10f;
    public float knockbackUpForce = 4f;
    public float stunDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isStunned;

    private Vector3 respawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        // fallback: posição inicial
        if (respawnPoint == Vector3.zero)
            respawnPoint = transform.position;

        if (lifeBar != null)
        {
            lifeBar.maxValue = maxHealth;
            lifeBar.value = currentHealth;
        }
    }

    // 🔑 CHAMADO PELO CHECKPOINT
    public void SetCheckpoint(Vector3 point)
    {
        respawnPoint = point;
        Debug.Log("Novo checkpoint salvo: " + respawnPoint);
    }

    public void TakeDamage(int amount, Transform enemy = null)
    {
        if (isStunned) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
        rb.angularVelocity = 0f;

        // move player
        transform.position = respawnPoint;

        // ignora limite da câmera
        PlayerMovement2D_TagBased move = GetComponent<PlayerMovement2D_TagBased>();
        if (move != null)
            move.IgnoreCameraLimit(0.15f);

        // reseta câmera
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
