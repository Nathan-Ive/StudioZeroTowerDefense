using UnityEngine;

/// <summary>
/// The Medieval Hooligan's clump of dirt. A homing projectile that slows the enemy it hits,
/// and can stop it completely for a moment while it cleans itself off.
///
/// It inherits from Projectile and only changes what a hit does (HitEnemy).
///
/// The Hooligan tower decides how strong the slow and stop are (SetUp).
///
/// Setup: use this component on the Hooligan's projectile prefab instead of Projectile.
/// </summary>
public class DirtProjectile : Projectile
{
    private float slowMultiplier = 1f;
    private float slowDuration;
    private float stunDuration;

    /// <summary>Called by the Hooligan right after spawning, before launching.</summary>
    public void SetUp(float newSlowMultiplier, float newSlowDuration, float newStunDuration)
    {
        slowMultiplier = newSlowMultiplier;
        slowDuration = newSlowDuration;
        stunDuration = newStunDuration;
    }

    protected override void HitEnemy(EnemyMovement enemy)
    {
        // The normal damage from Projectile first.
        base.HitEnemy(enemy);

        if (slowDuration > 0f)
        {
            enemy.ApplySlow(slowMultiplier, slowDuration);
        }

        if (stunDuration > 0f)
        {
            enemy.Stun(stunDuration);
        }
    }
}
