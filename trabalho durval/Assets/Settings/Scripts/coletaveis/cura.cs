using UnityEngine;

public class CollectableHeal : CollectableBase
{
    public int healAmount = 1;

    protected override void OnCollect(GameObject player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health == null) return;

        health.currentHealth = Mathf.Min(
            health.currentHealth + healAmount,
            health.maxHealth
        );

        health.SendMessage("UpdateUI", SendMessageOptions.DontRequireReceiver);
    }
}
