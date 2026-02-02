using UnityEngine;

public class FireballAzul : MonoBehaviour
{
    public float lifetime = 3f;
    public LayerMask hitLayers;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}
