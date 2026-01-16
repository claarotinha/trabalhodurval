using UnityEngine;

public class SlowCollectable : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float slowMultiplier = 1f;
    public float duration = 5f;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        // Ativa slow global
        if (EffectManager.Instance != null)
            EffectManager.Instance.ActivateSlow(slowMultiplier, duration);

        Destroy(gameObject);
    }
}
