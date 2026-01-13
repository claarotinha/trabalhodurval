using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool used = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().SetCheckpoint(transform.position);
            used = true;
        }
    }
}
