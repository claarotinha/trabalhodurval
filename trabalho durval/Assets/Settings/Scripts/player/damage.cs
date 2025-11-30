using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Valores de Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("UI")]
    public Slider lifeBar;   // Slider de vida

    void Start()
    {
        currentHealth = maxHealth;

        // Configura o slider no início
        if (lifeBar != null)
        {
            lifeBar.minValue = 0;
            lifeBar.maxValue = maxHealth;
            lifeBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateLifeUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        if (lifeBar != null)
        {
            lifeBar.value = currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("O Player morreu!");
        // Reiniciar cena
        // Ou chamar GameManager
        // Ou mostrar Game Over
    }
}
