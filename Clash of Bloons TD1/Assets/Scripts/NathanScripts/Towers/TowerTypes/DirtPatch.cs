using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The patch of dirt the Medieval Hooligan drops on the path.
/// Every enemy that walks into it is affected once (slow, and optionally damage and a stop).
/// After a set number of enemies, the patch is used up and disappears.
///
/// Size works like the tower range: the patch radius is half its X scale
/// (a Unity Cylinder or Sphere has radius 0.5 at scale 1). Height is ignored.
///
/// Setup:
/// 3D Object > Cylinder, scale about (3, 0.02, 3), brown material.
/// Add this component, remove the collider, save it as a prefab.
/// The Hooligan fills in the values through SetUp(), so you don't set them here.
/// </summary>
public class DirtPatch : MonoBehaviour
{
    private float slowMultiplier = 0.5f;
    private float slowDuration = 2f;
    private float damage = 0f;
    private float stunDuration = 0f;
    private int enemiesLeft = 10;

    // Enemies already affected, so standing in the patch counts as one enemy, not one per frame.
    private readonly HashSet<EnemyMovement> affected = new HashSet<EnemyMovement>();

    private float Radius { get { return transform.lossyScale.x * 0.5f; } }

    private void Awake()
    {
        // The patch works by distance, so a collider would only get in the way of clicks.
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    /// <summary>Called by the Hooligan right after spawning the patch.</summary>
    public void SetUp(float newSlowMultiplier, float newSlowDuration, float newDamage, float newStunDuration, int enemyLimit)
    {
        slowMultiplier = newSlowMultiplier;
        slowDuration = newSlowDuration;
        damage = newDamage;
        stunDuration = newStunDuration;
        enemiesLeft = Mathf.Max(1, enemyLimit);
    }

    private void Update()
    {
        List<EnemyMovement> enemies = EnemyMovement.ActiveEnemies;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            EnemyMovement enemy = enemies[i];
            if (enemy == null || affected.Contains(enemy))
            {
                continue;
            }

            Vector3 offset = enemy.transform.position - transform.position;
            offset.y = 0f;
            if (offset.sqrMagnitude > Radius * Radius)
            {
                continue;
            }

            Affect(enemy);

            enemiesLeft--;
            if (enemiesLeft <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    private void Affect(EnemyMovement enemy)
    {
        affected.Add(enemy);

        enemy.ApplySlow(slowMultiplier, slowDuration);

        if (stunDuration > 0f)
        {
            enemy.Stun(stunDuration);
        }

        if (damage > 0f)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
