using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Limite Esquerdo Progressivo")]
    public float leftLimit = 0f;
    public float limitPadding = 1f;

    private bool freezeCamera = false;

    void LateUpdate()
    {
        if (target == null || freezeCamera) return;

        // Atualiza limite APENAS se o player avançar
        if (target.position.x > leftLimit + limitPadding)
        {
            leftLimit = target.position.x - limitPadding;
        }

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.x = Mathf.Max(desiredPosition.x, leftLimit);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // Envia limite ao player
        if (target.TryGetComponent(out PlayerMovement2D_TagBased p))
        {
            p.cameraLeftLimit = leftLimit;
        }
    }

    // ===============================
    // 🔑 USADO NO RESPAWN
    // ===============================
    public void ForceCameraTo(Vector3 position)
    {
        freezeCamera = true;

        // Recalcula limite
        leftLimit = position.x - limitPadding;

        // Move câmera instantaneamente
        transform.position = new Vector3(
            position.x + offset.x,
            position.y + offset.y,
            transform.position.z
        );

        freezeCamera = false;
    }
}
