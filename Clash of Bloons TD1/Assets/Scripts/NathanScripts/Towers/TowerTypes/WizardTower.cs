using UnityEngine;

/// <summary>
/// WIZARD: unlocked by the Wizard Tower.
/// Throws a fireball in a straight line that pierces through several enemies, and from Level 2 sets them
/// on fire for damage over time (DoT). Only the direct damage gets upgraded, the DoT damage never does.
///
/// Needs a projectile prefab with the FireballProjectile component.
///
/// Base stats (set in its TowerData asset): ?? damage, ? second cooldown.
///
/// The fields below start at the Level 1 values. Level 1 has no burn, so to see the burn while testing,
/// set Burn Duration to 2 (the Level 2 value). The burn damage per second is not decided yet (default 1).
///
/// Planned upgrades (not built yet):
/// [Level 1]   Pierces 3 enemies,        0 seconds of DoT, 100% damage
/// [Level 2]   Pierces 5 enemies,        2 seconds of DoT, 110% damage
/// [Level 3]   Pierces 9 enemies,        4 seconds of DoT, 125% damage
/// [Max Level] Pierces infinite enemies, 8 seconds of DoT, 150% damage
/// </summary>
public class WizardTower : Tower
{
    [Header("Wizard")]
    [Tooltip("How many enemies one fireball can hit before it disappears (Level 1 = 3).")]
    [SerializeField] private int pierceCount = 3;

    [Tooltip("Tick for the Max Level 'pierces infinite enemies'.")]
    [SerializeField] private bool infinitePierce = false;

    [Tooltip("Seconds an enemy keeps burning (Level 1 = 0, Level 2 = 2).")]
    [SerializeField] private float burnDuration = 0f;

    [Tooltip("Burn damage, dealt once per second while burning.")]
    [SerializeField] private float burnDamagePerSecond = 1f;

    [Tooltip("How far the fireball flies, as a multiple of the range. Above 1 lets it carry on past the range edge.")]
    [SerializeField] private float travelDistanceMultiplier = 1.5f;

    protected override void Awake()
    {
        base.Awake();

        if (projectilePrefab != null && !(projectilePrefab is FireballProjectile))
        {
            Debug.LogError(name + ": the Wizard needs a projectile prefab with the FireballProjectile component.", this);
        }
    }

    protected override void Attack(EnemyMovement target)
    {
        // "as" turns the Projectile into a FireballProjectile, or gives null if it isn't one.
        Projectile spawned = SpawnProjectile();
        FireballProjectile fireball = spawned as FireballProjectile;
        if (fireball == null)
        {
            if (spawned != null) Destroy(spawned.gameObject);
            return;
        }

        fireball.SetUp(infinitePierce ? int.MaxValue : pierceCount, burnDamagePerSecond, burnDuration);
        fireball.LaunchInDirection(FlatDirectionTo(target), Damage, range.Radius * travelDistanceMultiplier);
    }
}
