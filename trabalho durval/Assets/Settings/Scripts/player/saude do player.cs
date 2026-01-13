using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")]
    public Slider lifeBar;

    [Header("Respawn")]
    public float respawnDelay = 1f;

    private Rigidbody2D rb;
    private PlayerMovement2D_TagBased movement;
    private CameraFollow cam;

    private bool isDead = false;
    private Vector3 checkpointPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement2D_TagBased>();
        cam = FindObjectOfType<CameraFollow>();

        currentHealth = maxHealth;
        checkpointPosition = transform.position;

        if (lifeBar != null)
        {
            lifeBar.maxValue = maxHealth;
            lifeBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        movement.enabled = false;
        rb.linearVelocity = Vector2.zero;

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Move player
        transform.position = checkpointPosition;
        rb.linearVelocity = Vector2.zero;

        // 🔑 MOVE A CÂMERA JUNTO
        if (cam != null)
            cam.ForceCameraTo(checkpointPosition);

        currentHealth = maxHealth;
        UpdateUI();

        movement.enabled = true;
        isDead = false;
    }

    public void SetCheckpoint(Vector3 pos)
    {
        checkpointPosition = pos;

        // Atualiza câmera imediatamente
        if (cam != null)
            cam.ForceCameraTo(pos);

        Debug.Log("Checkpoint definido: " + pos);
    }

    void UpdateUI()
    {
        if (lifeBar != null)
            lifeBar.value = currentHealth;
    }
}
