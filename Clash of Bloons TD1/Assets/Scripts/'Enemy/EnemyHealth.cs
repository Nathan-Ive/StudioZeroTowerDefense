using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    [Header("Regeneration settings")]
    public bool enableRegen = false;
    public float regenRate = 5f;

    [Header("Test info")]
    public float testDamageAmount = 10f;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        //debug feature to do some damage
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(testDamageAmount);
        }
        if (enableRegen && currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log(gameObject.name + " Has been damaged. Remaining health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    //regen if you guys want:)
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log(gameObject.name + " is Healing. Current health: " + currentHealth);
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " Has Died.");
        Destroy(gameObject);
    }
}