using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Limite Esquerdo Progressivo")]
    public float leftLimit = 0f;
    public float limitPadding = 1f;

    private bool skipFrame;

    void LateUpdate()
    {
        if (target == null) return;

        if (skipFrame)
        {
            skipFrame = false;
            return;
        }

        if (target.position.x > leftLimit + limitPadding)
            leftLimit = target.position.x - limitPadding;

        Vector3 desired = target.position + offset;
        desired.x = Mathf.Max(desired.x, leftLimit);

        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);

        if (target.TryGetComponent(out PlayerMovement2D_TagBased p))
            p.cameraLeftLimit = leftLimit;
    }

    public void ResetCameraTo(Vector3 point)
    {
        leftLimit = point.x - limitPadding;

        transform.position = new Vector3(
            point.x + offset.x,
            point.y + offset.y,
            transform.position.z
        );

        skipFrame = true;
    }
}
