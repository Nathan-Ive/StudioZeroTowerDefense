using UnityEngine;

/// <summary>
/// MEDIEVAL HOOLIGAN: unlocked by the Campsite. A support tower that slows enemies with dirt.
///
/// Two things it does:
/// 1. Normal attack (every few seconds): throws a clump of dirt at one enemy. Small damage,
///    and the enemy is slowed for a short time.
/// 2. Ability (every 15 seconds): drops a patch of dirt on the path where its target is standing.
///    Every enemy that walks through is slowed. The patch disappears after a set number of enemies.
///    Only one patch at a time, and the 15 second wait starts when a patch is thrown.
///
/// Base stats (set in its TowerData asset): 1 damage, 5 second cooldown.
/// A 50% enemy movement speed decrease for 2 seconds.
/// Needs a projectile prefab with the DirtProjectile component.
///
/// Planned upgrades for the patch (not built yet; the fields below already support them):
/// [Level 1]   Slows (no damage)                                      - gone after 10 enemies
/// [Level 2]   Slows + deals damage                                   - gone after 15 enemies
/// [Max Level] Stops for 2 s + permanently slows + deals damage       - gone after 20 enemies
///             (permanent = set Patch Slow Duration to a huge number, e.g. 9999)
/// </summary>
public class MedievalHooliganTower : Tower
{
    [Header("Dirt clump (normal attack)")]
    [Tooltip("Speed multiplier on the enemy that is hit. 0.5 = half speed.")]
    [SerializeField] private float hitSlowMultiplier = 0.5f;

    [Tooltip("Seconds the hit enemy stays slowed.")]
    [SerializeField] private float hitSlowDuration = 2f;

    [Tooltip("Seconds the hit enemy is stopped completely while it cleans itself off. 0 = no stop.")]
    [SerializeField] private float hitStunDuration = 0f;

    [Header("Dirt patch (ability)")]
    [Tooltip("Prefab with the DirtPatch component.")]
    [SerializeField] private DirtPatch dirtPatchPrefab;

    [Tooltip("Seconds between patches.")]
    [SerializeField] private float abilityCooldown = 15f;

    [Tooltip("Number of enemies that can walk through before the patch is gone (Level 1 = 10).")]
    [SerializeField] private int patchEnemyLimit = 10;

    [SerializeField] private float patchSlowMultiplier = 0.5f;
    [SerializeField] private float patchSlowDuration = 2f;

    [Tooltip("Damage to each enemy that walks in (Level 1 = 0).")]
    [SerializeField] private float patchDamage = 0f;

    [Tooltip("Seconds each enemy is stopped completely (Level 1 = 0, Max Level = 2).")]
    [SerializeField] private float patchStunDuration = 0f;

    [Tooltip("Used to drop the patch onto the ground under the target. Set it to the Ground layer.")]
    [SerializeField] private LayerMask groundLayer;

    private DirtPatch activePatch;
    private float abilityTimer;

    protected override void Awake()
    {
        base.Awake();

        if (projectilePrefab != null && !(projectilePrefab is DirtProjectile))
        {
            Debug.LogError(name + ": the Medieval Hooligan needs a projectile prefab with the DirtProjectile component.", this);
        }
    }

    protected override void OnPlaced()
    {
        abilityTimer = 0f; // first patch as soon as there is a target
    }

    protected override void Update()
    {
        base.Update();

        if (!IsPlaced)
        {
            return;
        }

        if (abilityTimer > 0f)
        {
            abilityTimer -= Time.deltaTime;
        }

        // "activePatch == null" is also true after the patch destroyed itself.
        if (abilityTimer <= 0f && activePatch == null && currentTarget != null)
        {
            ThrowDirtPatch(currentTarget);
            abilityTimer = abilityCooldown;
        }
    }

    protected override void Attack(EnemyMovement target)
    {
        // "as" turns the Projectile into a DirtProjectile, or gives null if it isn't one.
        Projectile spawned = SpawnProjectile();
        DirtProjectile dirt = spawned as DirtProjectile;
        if (dirt == null)
        {
            if (spawned != null) Destroy(spawned.gameObject);
            return;
        }

        dirt.SetUp(hitSlowMultiplier, hitSlowDuration, hitStunDuration);
        dirt.LaunchAt(target, Damage);
    }

    private void ThrowDirtPatch(EnemyMovement target)
    {
        if (dirtPatchPrefab == null)
        {
            Debug.LogWarning(name + " has no dirt patch prefab assigned.", this);
            return;
        }

        // Find the ground under the target: cast a ray straight down from above it.
        Vector3 spot = target.transform.position;
        if (Physics.Raycast(spot + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 20f, groundLayer))
        {
            spot = hit.point + Vector3.up * 0.02f; // slightly above the ground so it doesn't flicker
        }

        activePatch = Instantiate(dirtPatchPrefab, spot, Quaternion.identity);
        activePatch.SetUp(patchSlowMultiplier, patchSlowDuration, patchDamage, patchStunDuration, patchEnemyLimit);
    }
}
