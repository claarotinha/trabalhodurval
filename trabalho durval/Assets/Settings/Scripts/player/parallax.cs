using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Camera cam;

    [Range(0f, 1f)]
    public float parallaxFactor = 0.3f;

    private Vector3 startPos;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        startPos = transform.position;
    }

    void LateUpdate()
    {
        float camX = cam.transform.position.x;

        transform.position = new Vector3(
            startPos.x + camX * parallaxFactor,
            startPos.y,
            startPos.z
        );
    }
}
