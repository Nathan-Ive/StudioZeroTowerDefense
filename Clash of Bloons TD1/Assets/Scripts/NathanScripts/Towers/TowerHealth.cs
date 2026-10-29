using System.Collections.Generic;
using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public static readonly List<TowerHealth> ActiveTowers = new List<TowerHealth>();

    [Header("health")]
    public float maxHealth = 100f;
    private float currentHealth;

    void OnEnable()
    {
        ActiveTowers.Add(this);
    }
    void OnDisable()
    {
        ActiveTowers.Remove(this);
    }
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}