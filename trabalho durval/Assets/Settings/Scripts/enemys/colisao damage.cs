using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageCooldown = 1f; // tempo entre danos
    private bool canDamage = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamage(collision.collider);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    void TryDamage(Collider2D col)
    {
        if (!canDamage) return;
        if (!col.CompareTag("Player")) return;

        PlayerHealth hp = col.GetComponent<PlayerHealth>();

        if (hp != null)
        {
            hp.TakeDamage(damageAmount);
            StartCoroutine(DamageDelay());
        }
    }

    private System.Collections.IEnumerator DamageDelay()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
