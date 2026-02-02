using UnityEngine;

public class NPCBalão : MonoBehaviour
{
    public GameObject balaoPrefab; // Arraste o prefab do balão aqui
    
    private GameObject balaoAtual;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && balaoPrefab != null)
        {
            // Cria o balão se não existe
            if (balaoAtual == null)
            {
                balaoAtual = Instantiate(balaoPrefab);
            }
            
            // Coloca acima do NPC
            balaoAtual.transform.position = transform.position + Vector3.up * 2f;
            balaoAtual.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && balaoAtual != null)
        {
            balaoAtual.SetActive(false);
        }
    }
}