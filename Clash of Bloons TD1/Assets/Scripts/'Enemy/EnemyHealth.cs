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

    // Burning, applied by the Wizard.
    private float burnDamagePerSecond = 0f;
    private float burnTimer = 0f;      // seconds of burning left
    private float burnTickTimer = 0f;  // seconds until the next burn damage

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead) return;

        // Burning deals damage once per second while it lasts.
        // Once per second (not every frame).
        if (burnTimer > 0f)
        {
            burnTimer -= Time.deltaTime;
            burnTickTimer -= Time.deltaTime;
            if (burnTickTimer <= 0f)
            {
                burnTickTimer += 1f;
                TakeDamage(burnDamagePerSecond);
                if (isDead) return;
            }
        }

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

    /// <summary>Set this enemy on fire. A new burn refreshes the duration and uses the new damage.</summary>
    public void ApplyBurn(float damagePerSecond, float duration)
    {
        if (isDead || damagePerSecond <= 0f || duration <= 0f) return;

        if (burnTimer <= 0f)
        {
            burnTickTimer = 1f; // first burn damage one second after catching fire
        }
        burnDamagePerSecond = damagePerSecond;
        burnTimer = Mathf.Max(burnTimer, duration);
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " Has Died.");
        Destroy(gameObject);
    }
}