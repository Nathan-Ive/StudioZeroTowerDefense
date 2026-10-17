using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base class every tower inherits from. It holds everything towers have in common.
///
/// What it does so far:
///   Attack an enemy (spawn a projectile).
///   Shoot from a chosen point on the tower's body (firePoint) and turn to face the target.
///   Flying towards the target. This change was made in projectile.
///   Bullet disappearing when it reaches its target. This change was made in projectile.
///   Cooldown and automatic shooting.
///   Only shoot targets within range.
///   Target Priority (Closest, First, Last).
///
///
/// 
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

    [Header("Range")]
    [Tooltip("The child sphere with the TowerRange component. Found automatically if left empty.")]
    [SerializeField] protected TowerRange range;

    [Header("Projectile")]
    [Tooltip("Prefab with the Projectile component that this tower shoots.")]
    [SerializeField] protected Projectile projectilePrefab;

    [Header("Targeting")]
    [Tooltip("Which enemy to attack when several are in range. You can change it in the Inspector during Play.")]
    [SerializeField] protected TargetPriority priority = TargetPriority.First;

    protected EnemyMovement currentTarget;

    private float cooldownTimer;     // seconds until the next attack is allowed
    
    private float retargetTimer;     // seconds until the tower re-checks which enemy is the best target
    private const float RetargetInterval = 0.2f;

    // ---------------------------------------------------------------------------------
    // Read-only information for other scripts.
    // ---------------------------------------------------------------------------------

    public TowerData Data { get { return data; } }
    public TargetPriority Priority { get { return priority; } }

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

        if (range == null)
        {
            range = GetComponentInChildren<TowerRange>();
        }
        if (range == null)
        {
            Debug.LogError(name + " has no child with a TowerRange component, so it is switched off.", this);
            enabled = false;
            return;
        }

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    protected virtual void Start()
    {
        ShowRange(false);
    }

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        retargetTimer -= Time.deltaTime;
        if (retargetTimer <= 0f || !IsValidTarget(currentTarget))
        {
            retargetTimer = RetargetInterval;
            currentTarget = FindBestTarget();
        }

        if (currentTarget == null)
        {
            return;
        }

        RotateTowards(currentTarget);

        if (cooldownTimer <= 0f)
        {
            Attack(currentTarget);
            cooldownTimer = data.attackCooldown;
        }
    }

    /// <summary>Show or hide the range sphere. Hidden does not turn the range off.</summary>
    public void ShowRange(bool visible)
    {
        if (range != null)
        {
            range.SetVisible(visible);
        }
    }

    // ---------------------------------------------------------------------------------
    // Targeting
    // ---------------------------------------------------------------------------------

    /// <summary>Go to the next priority: First, then Last, then Closest, then back to First.</summary>
    public void CyclePriority()
    {
        if (priority == TargetPriority.First)
        {
            priority = TargetPriority.Last;
        }
        else if (priority == TargetPriority.Last)
        {
            priority = TargetPriority.Closest;
        }
        else
        {
            priority = TargetPriority.First;
        }

        retargetTimer = 0f; // pick a new target on the next frame
    }

    /// <summary>A target is valid while it still exists and stands inside the range.</summary>
    protected bool IsValidTarget(EnemyMovement enemy)
    {
        // "enemy != null" is also false for an enemy that was destroyed (Unity handles that for us).
        return enemy != null && range.Contains(enemy.transform.position);
    }

    /// <summary>The best enemy in range according to the priority, or null if none.</summary>
    protected EnemyMovement FindBestTarget()
    {
        List<EnemyMovement> enemies = GetEnemiesInRange();
        if (enemies.Count == 0)
        {
            return null;
        }
        return enemies[0];
    }

    /// <summary>Every enemy inside the range, sorted with the best target first.</summary>
    protected List<EnemyMovement> GetEnemiesInRange()
    {
        List<EnemyMovement> inRange = new List<EnemyMovement>();

        // EnemyMovement keeps a list of every living enemy, so we never have to search the whole scene.
        foreach (EnemyMovement enemy in EnemyMovement.ActiveEnemies)
        {
            if (enemy != null && range.Contains(enemy.transform.position))
            {
                inRange.Add(enemy);
            }
        }

        SortByPriority(inRange);
        return inRange;
    }

    private void SortByPriority(List<EnemyMovement> enemies)
    {
        // Sort takes a comparison: a negative result puts "a" before "b".
        if (priority == TargetPriority.First)
        {
            // Least distance left to walk = closest to the end = first in the list.
            enemies.Sort((a, b) => a.DistanceToEnd.CompareTo(b.DistanceToEnd));
        }
        else if (priority == TargetPriority.Last)
        {
            // Most distance left to walk first.
            enemies.Sort((a, b) => b.DistanceToEnd.CompareTo(a.DistanceToEnd));
        }
        else
        {
            // Closest to this tower first.
            enemies.Sort((a, b) => DistanceTo(a).CompareTo(DistanceTo(b)));
        }
    }

    private float DistanceTo(EnemyMovement enemy)
    {
        return Vector3.Distance(transform.position, enemy.transform.position);
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
