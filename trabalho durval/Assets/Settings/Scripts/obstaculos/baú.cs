using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    private Animator anim;
    private bool opened = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !opened)
        {
            anim.SetBool("Open", true);
            opened = true;
        }
    }
}
