using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Referência")]
    public Transform player;

    [Header("Suavização")]
    public float smoothSpeed = 6f;
    public Vector3 offset;

    [Header("Limite Esquerdo Progressivo")]
    public float leftLimit = 0f;
    public float limitPadding = 1f;

    private Vector3 velocity = Vector3.zero;
    private bool forceSnap = false;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        Vector3 startPos = player.position + offset;
        startPos.x = Mathf.Max(startPos.x, leftLimit);

        transform.position = new Vector3(
            startPos.x,
            startPos.y,
            transform.position.z
        );
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Atualiza limite progressivo
        if (player.position.x > leftLimit + limitPadding)
            leftLimit = player.position.x - limitPadding;

        Vector3 desiredPos = player.position + offset;
        desiredPos.x = Mathf.Max(desiredPos.x, leftLimit);

        if (forceSnap)
        {
            transform.position = new Vector3(
                desiredPos.x,
                desiredPos.y,
                transform.position.z
            );

            velocity = Vector3.zero;
            forceSnap = false;
            return;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            new Vector3(desiredPos.x, desiredPos.y, transform.position.z),
            ref velocity,
            1f / smoothSpeed
        );
    }

    // ===============================
    // 🔑 USADO PELO PLAYER
    // ===============================
    public float GetLeftLimit()
    {
        return leftLimit;
    }

    // ===============================
    // 🔑 RESPAWN DA CÂMERA
    // ===============================
    public void ResetCameraTo(Vector3 point)
    {
        leftLimit = point.x - limitPadding;

        transform.position = new Vector3(
            point.x + offset.x,
            point.y + offset.y,
            transform.position.z
        );

        velocity = Vector3.zero;
        forceSnap = true;
    }
}
