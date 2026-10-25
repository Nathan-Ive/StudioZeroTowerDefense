using UnityEngine;

/// <summary>
/// The Wizard's fireball.
///
/// It inherits from StraightProjectile, which inherits from Projectile:
///   From Projectile:          speed, damage, hitting.
///   From StraightProjectile:  flying in a straight line.
///   Added here:               piercing through several enemies, and setting them on fire.
///
/// The Wizard tower decides how many enemies it pierces and how long they burn (SetUp),
/// so upgrades only need to change the Wizard's values, not this script.
///
/// Setup: use this component on the Wizard's projectile prefab instead of Projectile.
/// </summary>
public class FireballProjectile : StraightProjectile
{
    private float burnDamagePerSecond;
    private float burnDuration;

    /// <summary>Called by the Wizard right after spawning, before launching.</summary>
    public void SetUp(int pierceCount, float newBurnDamagePerSecond, float newBurnDuration)
    {
        hitsLeft = Mathf.Max(1, pierceCount);
        burnDamagePerSecond = newBurnDamagePerSecond;
        burnDuration = newBurnDuration;
    }

    protected override void HitEnemy(EnemyMovement enemy)
    {
        // The normal damage from Projectile first.
        base.HitEnemy(enemy);

        if (burnDuration > 0f)
        {
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.ApplyBurn(burnDamagePerSecond, burnDuration);
            }
        }
    }
}
