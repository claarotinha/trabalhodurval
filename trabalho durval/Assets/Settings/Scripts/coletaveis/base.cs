using UnityEngine;

public class CollectableBase : MonoBehaviour
{
    protected bool collected = false;
    private Vector3 startPosition;

    protected virtual void Start()
    {
        startPosition = transform.position;

        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.RegisterCollectable(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;
        OnCollect(other.gameObject);

        // Não destruímos, apenas desativamos
        gameObject.SetActive(false);
    }

    protected virtual void OnCollect(GameObject player)
    {
        // Implementado nos filhos
    }

    public void Respawn()
    {
        collected = false;
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
}
