using UnityEngine;

public class CollectableHeal : MonoBehaviour
{
    public int healAmount = 1;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        PlayerHealth health = col.GetComponent<PlayerHealth>();
        if (health == null) return;

        health.currentHealth = Mathf.Min(
            health.currentHealth + healAmount,
            health.maxHealth
        );

        // força atualização
        health.SendMessage("UpdateUI", SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }
}
