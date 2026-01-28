using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.SetCheckpoint(transform.position);
                activated = true;
                Debug.Log("Checkpoint ativado: " + transform.position);
            }
        }
    }
}
