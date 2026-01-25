using UnityEngine;

public class JumpCollectable : MonoBehaviour
{
    public float jumpMultiplier = 1.5f;
    public float duration = 5f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        // Ativa efeito global de jump boost
        if (EffectManager.Instance != null)
            EffectManager.Instance.ActivateJumpBoost(jumpMultiplier, duration);

        // Coletável some
        Destroy(gameObject);
    }
}
