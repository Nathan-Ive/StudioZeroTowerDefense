using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ARCHER: unlocked by the Watchtower.
/// Fires several arrows per attack, in one of two modes the player switches in the tower info panel.
///
/// - Spread: arrows fly in a straight fan towards the target. Each arrow hits the first enemy it passes through.
///           Good against groups walking close together.
/// - Homing: arrows lock onto enemies and can't miss. With several enemies in range, each arrow picks the
///           next-best target, so they spread over the group.
///
/// Every arrow deals the same damage. Upgrades will add arrows, not damage.
/// Needs a projectile prefab with the StraightProjectile component (it can fly straight and homing).
///
/// Base stats (set in its TowerData asset): ?? damage per arrow, ? second cooldown.
///
/// Planned upgrades (not built yet):
/// [Level 1]   3 arrows (Spread)  - 2 arrows (Homing)
/// [Level 2]   5 arrows (Spread)  - 4 arrows (Homing)
/// [Level 3]   7 arrows (Spread)  - 6 arrows (Homing)
/// [Max Level] 10 arrows (Spread) - 8 arrows (Homing)
/// </summary>
public class ArcherTower : Tower
{
    public enum ArcherMode { Spread, Homing }

    [Header("Archer")]
    [SerializeField] private ArcherMode mode = ArcherMode.Spread;

    [Tooltip("Arrows per attack in Spread mode (Level 1 = 3).")]
    [SerializeField] private int spreadArrowCount = 3;

    [Tooltip("Total width of the fan in degrees.")]
    [SerializeField] private float spreadAngle = 30f;

    [Tooltip("Arrows per attack in Homing mode (Level 1 = 2).")]
    [SerializeField] private int homingArrowCount = 2;

    protected override void Awake()
    {
        base.Awake();

        if (projectilePrefab != null && !(projectilePrefab is StraightProjectile))
        {
            Debug.LogError(name + ": the Archer needs a projectile prefab with the StraightProjectile component.", this);
        }
    }

    // ---------------------------------------------------------------------------------
    // Mode switching (used by the tower info panel)
    // ---------------------------------------------------------------------------------

    public override bool HasMode { get { return true; } }

    public override string ModeLabel { get { return mode.ToString(); } }

    public override void ToggleMode()
    {
        if (mode == ArcherMode.Spread)
        {
            mode = ArcherMode.Homing;
        }
        else
        {
            mode = ArcherMode.Spread;
        }
    }

    // ---------------------------------------------------------------------------------
    // Attacking
    // ---------------------------------------------------------------------------------

    protected override void Attack(EnemyMovement target)
    {
        if (mode == ArcherMode.Spread)
        {
            FireSpread(target);
        }
        else
        {
            FireHoming(target);
        }
    }

    private void FireSpread(EnemyMovement target)
    {
        Vector3 towardsTarget = FlatDirectionTo(target);

        for (int i = 0; i < spreadArrowCount; i++)
        {
            // Spread the arrows evenly from -half to +half of the fan. With 3 arrows and 30 degrees: -15, 0, +15.
            float t = spreadArrowCount == 1 ? 0.5f : i / (float)(spreadArrowCount - 1);
            float angle = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, t);

            // Rotating a direction: multiply a rotation (Quaternion) by the direction.
            Vector3 arrowDirection = Quaternion.Euler(0f, angle, 0f) * towardsTarget;

            // "as" turns the Projectile into a StraightProjectile, or gives null if it isn't one.
            Projectile spawned = SpawnProjectile();
            StraightProjectile arrow = spawned as StraightProjectile;
            if (arrow == null)
            {
                if (spawned != null) Destroy(spawned.gameObject);
                return;
            }

            // Arrows fly as far as the edge of the range, then disappear.
            arrow.LaunchInDirection(arrowDirection, Damage, range.Radius);
        }
    }

    private void FireHoming(EnemyMovement mainTarget)
    {
        // Sorted by priority, best first.
        List<EnemyMovement> targets = GetEnemiesInRange();

        for (int i = 0; i < homingArrowCount; i++)
        {
            // Arrow 1 takes the best target, arrow 2 the second best, and so on.
            // With fewer enemies than arrows, it starts again from the best one (that's what % does).
            EnemyMovement arrowTarget = targets.Count > 0 ? targets[i % targets.Count] : mainTarget;

            Projectile arrow = SpawnProjectile();
            if (arrow != null)
            {
                arrow.LaunchAt(arrowTarget, Damage);
            }
        }
    }
}
