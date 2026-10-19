using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The base class every tower inherits from.
/// 
/// It is "abstract", so you can never put a plain Tower in the scene: every tower must be a specific type, like SoldierTower. 
/// 
/// A child class changes behaviour by overriding a "virtual" method, usually Attack(). 
/// Anything it does not override, it inherits.
///
/// For now the tower attacks when you press Space for testing.
/// I'll replaces this with a cooldown after setting up other content.
/// </summary>

public abstract class Tower : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("The TowerData asset with this tower's stats.")]
    [SerializeField] protected TowerData data;

    [Header("Projectile")]
    [Tooltip("Prefab with the Projectile component that this tower shoots.")]
    [SerializeField] protected Projectile projectilePrefab;

    protected EnemyMovement currentTarget;

    // ---------------------------------------------------------------------------------
    // Read-only information for other scripts.
    // ---------------------------------------------------------------------------------

    public TowerData Data { get { return data; } }

    /// <summary>Damage of one hit, from TowerData. Used by the fired projectiles from this tower.</summary>
    public float Damage
    {
        get { return data.damage; }
    }

    // ---------------------------------------------------------------------------------
    // Unity messages
    // ---------------------------------------------------------------------------------

    protected virtual void Awake()
    {
        if (data == null)
        {
            Debug.LogError(name + " has no TowerData assigned, so it is switched off.", this);
            enabled = false;
            return;
        }
    }

    protected virtual void Update()
    {
        // Pick a target when we don't have one (or it was destroyed).
        if (currentTarget == null)
        {
            currentTarget = FindBestTarget();
        }

        if (currentTarget == null)
        {
            return;
        }

        // TEMPORARY
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Attack(currentTarget);
        }
    }

    // ---------------------------------------------------------------------------------
    // Targeting
    // ---------------------------------------------------------------------------------

    /// <summary>
    /// The enemy to attack. For now that is simply the first living enemy, wherever it is.
    /// </summary>
    protected EnemyMovement FindBestTarget()
    {
        // EnemyMovement keeps a list of every living enemy, so we never have to search the whole scene.
        foreach (EnemyMovement enemy in EnemyMovement.ActiveEnemies)
        {
            if (enemy != null)
            {
                return enemy;
            }
        }
        return null;
    }

    // ---------------------------------------------------------------------------------
    // Attacking
    // ---------------------------------------------------------------------------------

    /// <summary>
    /// The default attack: launch one projectile at the target.
    /// A child class can override this to attack in its own way.
    /// </summary>
    protected virtual void Attack(EnemyMovement target)
    {
        Projectile projectile = SpawnProjectile();
        if (projectile != null)
        {
            projectile.LaunchAt(target, Damage);
        }
    }

    /// <summary>Spawn this tower's projectile at the tower. Does not launch it yet.</summary>
    protected Projectile SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning(name + " has no projectile prefab assigned.", this);
            return null;
        }
        return Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}
