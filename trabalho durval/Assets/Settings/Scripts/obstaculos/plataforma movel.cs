using UnityEngine;

public class PlatformMoverSimple : MonoBehaviour
{
    [Header("Pontos de movimento")]
    public Vector3 pointA; // Define pelo Inspector
    public Vector3 pointB; // Define pelo Inspector

    [Header("Velocidade")]
    public float moveSpeed = 2f;

    private Vector3 targetPos;

    void Start()
    {
        // Começa no ponto A
        transform.position = pointA;
        targetPos = pointB;
    }

    void FixedUpdate()
    {
        // Move suavemente para o ponto alvo
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);

        // Alterna entre A e B quando chega perto do destino
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            targetPos = (targetPos == pointA) ? pointB : pointA;
        }
    }

    // Faz o player se mover junto com a plataforma
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.transform.SetParent(null);
    }
}
