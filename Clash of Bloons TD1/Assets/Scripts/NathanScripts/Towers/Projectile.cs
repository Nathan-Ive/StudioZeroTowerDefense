using UnityEngine;

/// <summary>
/// Anything a tower shoots: swords, arrows, fireballs, dirt, baguettes.
///
/// It doesn't hit yet: it reaches the enemy and sticks to it.
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Units per second.")]
    [SerializeField] private float speed = 20f;

    // Set when it is launched
    private float damage;
    private EnemyMovement target;            // The enemy it flies to

    // ---------------------------------------------------------------------------------
    // Launching
    // ---------------------------------------------------------------------------------

    /// <summary>Give the projectile its target and the damage it carries.</summary>
    public void LaunchAt(EnemyMovement newTarget, float newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    // ---------------------------------------------------------------------------------
    // Flying
    // ---------------------------------------------------------------------------------

    private void Update()
    {
        // Nothing to fly to: not launched yet, or the enemy was destroyed.
        if (target == null)
        {
            return;
        }

        Vector3 aimPoint = GetAimPoint(target);
        transform.position = Vector3.MoveTowards(transform.position, aimPoint, speed * Time.deltaTime);

        Vector3 toTarget = aimPoint - transform.position;
        if (toTarget.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(toTarget);
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
