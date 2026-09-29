using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private GameScreenManager gameManager;

    void Start()
    {
        currentHealth = maxHealth;
        gameManager = FindObjectOfType<GameScreenManager>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameManager != null)
        {
            gameManager.TriggerGameOver();
        }
        Destroy(gameObject);
    }
}