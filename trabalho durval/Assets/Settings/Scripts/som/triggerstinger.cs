using UnityEngine;
using UnityEngine.Events;

public class StingerTrigger : MonoBehaviour
{
    public UnityEvent onEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            onEnter.Invoke();
    }
}
