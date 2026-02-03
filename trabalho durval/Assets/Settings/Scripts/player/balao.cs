using UnityEngine;

public class PlacaInterativa : MonoBehaviour
{
    public GameObject balaoUI;

    private void Start()
    {
        if (balaoUI != null)
            balaoUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (balaoUI != null)
            {
                balaoUI.SetActive(true);
           
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (balaoUI != null)
                balaoUI.SetActive(false);
        }
    }
}