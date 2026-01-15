using UnityEngine;

public class SafeZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            EnemySpawner.inZonaSegura = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            EnemySpawner.inZonaSegura = false;
    }
}
