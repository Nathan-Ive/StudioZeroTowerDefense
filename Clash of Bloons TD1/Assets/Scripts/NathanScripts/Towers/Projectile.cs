using UnityEngine;

/// <summary>
/// Anything a tower shoots: swords, arrows, fireballs, dirt, baguettes.
///
/// For now it only remembers its target and its damage, and stays where it was spawned.
/// </summary>
public class Projectile : MonoBehaviour
{
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
}
