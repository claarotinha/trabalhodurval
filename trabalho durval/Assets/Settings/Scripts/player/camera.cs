using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Player
    public float smoothSpeed = 5f; // Suavização da câmera
    public Vector3 offset;         // Distância da câmera para o player

    [Header("Limite Esquerdo")]
    public float minCameraX = 0f;  // câmera não pode ir para trás deste valor

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // 🔒 Trava da esquerda
        desiredPosition.x = Mathf.Max(desiredPosition.x, minCameraX);

        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}
