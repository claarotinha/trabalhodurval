using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Suavização")]
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Limite Esquerdo Progressivo")]
    public float leftLimit = 0f;
    public float limitPadding = 1f;

    private bool forceSnap;

    void Start()
    {
        if (target == null) return;

        Vector3 initialPos = target.position + offset;
        initialPos.x = Mathf.Max(initialPos.x, leftLimit);

        transform.position = new Vector3(
            initialPos.x,
            initialPos.y,
            transform.position.z
        );

        forceSnap = true;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Atualiza limite esquerdo progressivo
        if (target.position.x > leftLimit + limitPadding)
        {
            leftLimit = target.position.x - limitPadding;
        }

        Vector3 desired = target.position + offset;
        desired.x = Mathf.Max(desired.x, leftLimit);

        if (forceSnap)
        {
            transform.position = new Vector3(
                desired.x,
                desired.y,
                transform.position.z
            );

            forceSnap = false;
            return;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smoothSpeed * Time.deltaTime
        );
    }

    public void ResetCameraTo(Vector3 point)
    {
        leftLimit = point.x - limitPadding;

        transform.position = new Vector3(
            point.x + offset.x,
            point.y + offset.y,
            transform.position.z
        );

        forceSnap = true;
    }
}
