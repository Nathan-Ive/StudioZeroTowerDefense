using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The base class every tower inherits from. It holds everything towers have in common.
///
/// What it does so far:
///   Attack an enemy (spawn a projectile)
///   Shoot from a chosen point on the tower's body (firePoint) and turn to face the target
///   Flying towards the target. This change was made in projectile.
///
/// It is "abstract", so you can never put a plain Tower in the scene: every tower must be a
/// specific type, like SoldierTower. A child class changes behaviour by overriding a "virtual"
/// method, usually Attack().
/// </summary>
public abstract class Tower : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("The TowerData asset with this tower's stats.")]
    [SerializeField] protected TowerData data;

    [Header("Body parts")]
    [Tooltip("Empty child where projectiles spawn, e.g. the Soldier's hand or the Wizard's staff. Leave empty to shoot from the tower's centre.")]
    [SerializeField] protected Transform firePoint;

    [Tooltip("The part that turns to face the target (e.g. the upper body). Put the Fire Point inside it so it turns along. Leave empty for no turning.")]
    [SerializeField] protected Transform partToRotate;

    [Tooltip("How quickly the rotating part turns towards its target.")]
    [SerializeField] protected float turnSpeed = 10f;

    [Header("Projectile")]
    [Tooltip("Prefab with the Projectile component that this tower shoots.")]
    [SerializeField] protected Projectile projectilePrefab;

    protected EnemyMovement currentTarget;

    // ---------------------------------------------------------------------------------
    // Read-only information for other scripts.
    // ---------------------------------------------------------------------------------

    public TowerData Data { get { return data; } }

    /// <summary>Damage of one hit, from TowerData. Every projectile this tower fires uses it.</summary>
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


        if (firePoint == null)
        {
            firePoint = transform;
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


        RotateTowards(currentTarget);

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
    /// Turn the rotating part to face the target, on the ground plane only, so the tower never tips over.
    /// </summary>
    protected void RotateTowards(EnemyMovement target)
    {
        if (partToRotate == null)
        {
            return;
        }

        Vector3 direction = target.transform.position - partToRotate.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        partToRotate.rotation = Quaternion.Slerp(partToRotate.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

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

    /// <summary>Spawn this tower's projectile at the fire point. Does not launch it yet.</summary>
    protected Projectile SpawnProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning(name + " has no projectile prefab assigned.", this);
            return null;
        }

        return Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
