using UnityEngine;

/// <summary>
/// Anything a tower shoots: swords, arrows, fireballs, dirt, baguettes.
///
/// It follows one enemy until it reaches it, so it can't miss.
/// If the enemy dies mid-flight, it flies to where the enemy was and disappears.
///
/// Hits are found by distance, not by Unity physics. That means projectiles need NO collider
/// and NO Rigidbody, and they cannot get stuck on the road, the fence or each other.
/// 
/// This is important, because they will run into issues if they do.
/// </summary>
public class Projectile : MonoBehaviour
{
    // How fast it flies
    [Header("Movement")]
    [Tooltip("Units per second.")]
    [SerializeField] private float speed = 20f;

    // How close counts as a hit, and a safety timer
    [Tooltip("How close the projectile must get to an enemy to count as a hit.")]
    [SerializeField] private float hitRadius = 0.6f;

    [Tooltip("Safety net: the projectile is removed after this many seconds, whatever happens.")]
    [SerializeField] private float maxLifetime = 5f;

    // Set when it is launched
    private bool launched;
    private float damage;
    private EnemyMovement target;
    private Vector3 lastKnownTargetPoint;
    private float lifeTimer;

    // ---------------------------------------------------------------------------------
    // Launching
    // ---------------------------------------------------------------------------------

    /// <summary>Homing: fly to this enemy and hit it.</summary>
    public void LaunchAt(EnemyMovement newTarget, float newDamage)
    {
        target = newTarget;
        damage = newDamage;

        lastKnownTargetPoint = GetAimPoint(newTarget);
        lifeTimer = 0f;
        launched = true;
    }


    // ---------------------------------------------------------------------------------
    // Flying
    // ---------------------------------------------------------------------------------

    private void Update()
    {
        if (!launched)
        {
            return;
        }

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifetime)
        {
            Destroy(gameObject);
            return;
        }

        UpdateHoming();
    }

    private void UpdateHoming()
    {
        if (target != null)
        {
            lastKnownTargetPoint = GetAimPoint(target);
        }

        Vector3 toTarget = lastKnownTargetPoint - transform.position;
        float step = speed * Time.deltaTime;


        if (toTarget.magnitude <= Mathf.Max(step, hitRadius))
        {
            if (target != null)
            {
                HitEnemy(target);
            }
            Destroy(gameObject);
            return;
        }

        transform.position += toTarget.normalized * step;
        transform.rotation = Quaternion.LookRotation(toTarget);
    }


    // ---------------------------------------------------------------------------------
    // Hitting
    // ---------------------------------------------------------------------------------

    private void HitEnemy(EnemyMovement enemy)
    {
        // Health is taken from EnemyHealth.cs.
        Health health = enemy.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }


    /// <summary>
    /// The point to aim at on an enemy: the middle of its collider if it has one,
    /// otherwise its pivot. Without this, projectiles aim at the enemy's feet.
    /// </summary>
    public static Vector3 GetAimPoint(EnemyMovement enemy)
    {
        Collider enemyCollider = enemy.GetComponent<Collider>();
        if (enemyCollider != null && enemyCollider.enabled)
        {
            return enemyCollider.bounds.center;
        }
        return enemy.transform.position;
    }
}
