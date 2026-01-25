using UnityEngine;

public class CollectableBase : MonoBehaviour
{
    protected bool collected = false;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;
        OnCollect(other.gameObject);
        Destroy(gameObject);
    }

    protected virtual void OnCollect(GameObject player)
    {
        // Implementado nos filhos
    }
}
