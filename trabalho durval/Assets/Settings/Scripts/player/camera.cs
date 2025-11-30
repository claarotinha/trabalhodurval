using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Limite Esquerdo Progressivo")]
    public float leftLimit = 0f;   // cresce conforme o player avança
    public float limitPadding = 1f; // quão colado o player precisa estar para avançar o limite

    void LateUpdate()
    {
        if (target == null) return;

        // Atualiza limite conforme player avança
        if (target.position.x > leftLimit + limitPadding)
        {
            leftLimit = target.position.x - limitPadding;
        }

        // Calcula posição desejada
        Vector3 desiredPosition = target.position + offset;

        // Trava esquerda da câmera
        desiredPosition.x = Mathf.Max(desiredPosition.x, leftLimit);

        // Suavização
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;

        // --- ENVIA O LIMITE PARA O PLAYER --- //
        if (target.TryGetComponent<PlayerMovement2D_TagBased>(out var p))
        {
            p.cameraLeftLimit = leftLimit;
        }
    }
}
