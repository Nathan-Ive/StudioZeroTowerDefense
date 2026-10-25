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
///   Each tower type can change its attack by overriding Attack().
///   The Baker's damage bonus is added to every tower's damage.
///   Modes the player can switch in the info panel (only the Archer has one).
///
///
/// It is "abstract", so you can never put a plain Tower in the scene: every tower must be a
/// specific type, like SoldierTower. A child class changes behaviour by overriding a "virtual"
/// method, usually Attack(). Anything it does not override, it inherits.
///
///   1. TowerPlacer spawns a tower and calls BeginPreview(): it follows the mouse, and shows its range.
///   2. The player clicks: TowerPlacer calls Place(). The range hides and the tower starts fighting.
///   A tower dragged into the scene by hand in the editor places itself in Start() (for testing).
/// </summary>
public abstract class Tower : MonoBehaviour
{
    /// <summary>Every placed tower in the scene. Not used yet; ready for selling, upgrades or saving later.</summary>
    public static readonly List<Tower> PlacedTowers = new List<Tower>();

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
    [Tooltip("Which enemy to attack when several are in range. The player can change it in the tower info panel.")]
    [SerializeField] protected TargetPriority priority = TargetPriority.First;

    protected EnemyMovement currentTarget;

    private float cooldownTimer;     // seconds until the next attack is allowed

    private float retargetTimer;     // seconds until the tower re-checks which enemy is the best target
    private const float RetargetInterval = 0.2f;

    //Placement state
    private bool isPreview;
    private bool isPlaced;

    // ---------------------------------------------------------------------------------
    // Read-only information for other scripts.
    // ---------------------------------------------------------------------------------

    public TowerData Data { get { return data; } }
    public TowerRange Range { get { return range; } }
    public TargetPriority Priority { get { return priority; } }
    public bool IsPlaced { get { return isPlaced; } }

    /// <summary>
    /// Damage of one hit right now: the base damage from TowerData plus any Baker bonus.
    /// Every projectile this tower fires uses it.
    /// </summary>
    public float Damage
    {
        get { return data.damage + BakerTower.GetDamageBonusFor(this); }
    }

    // Modes, used by the tower info panel. Only the Archer overrides these.

    /// <summary>Does this tower have a mode the player can switch?</summary>
    public virtual bool HasMode { get { return false; } }

    /// <summary>Name of the current mode, shown on the mode button.</summary>
    public virtual string ModeLabel { get { return ""; } }

    /// <summary>Switch to the next mode. Does nothing unless a child class overrides it.</summary>
    public virtual void ToggleMode() { }

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
        // TowerPlacer calls BeginPreview() before Start.
        // If it did not, the tower was placed by hand in the editor, so it will start fighting straight away.
        if (!isPreview)
        {
            Place();
        }
    }

    protected virtual void Update()
    {
        if (!isPlaced)
        {
            return;
        }

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

    protected virtual void OnDestroy()
    {
        PlacedTowers.Remove(this);
    }

    // ---------------------------------------------------------------------------------
    // Placement (used by TowerPlacer)
    // ---------------------------------------------------------------------------------

    /// <summary>Called right after spawning: the tower follows the mouse and does not fight.</summary>
    public void BeginPreview()
    {
        isPreview = true;
        isPlaced = false;
        ShowRange(true);
    }

    /// <summary>Called when the player confirms the spot. The tower starts fighting.</summary>
    public void Place()
    {
        isPreview = false;
        if (isPlaced)
        {
            return;
        }

        isPlaced = true;
        PlacedTowers.Add(this);
        ShowRange(false);
        cooldownTimer = 0f; // first attack as soon as an enemy walks into range
        OnPlaced();
    }

    /// <summary>For child classes that need to do something once placed (the Baker and the Medieval Hooligan).</summary>
    protected virtual void OnPlaced() { }

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

    /// <summary>
    /// Every enemy inside the range, sorted with the best target first.
    /// The Archer's homing mode uses the whole list to spread its arrows over several enemies.
    /// </summary>
    protected List<EnemyMovement> GetEnemiesInRange()
    {
        List<EnemyMovement> inRange = new List<EnemyMovement>();

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

    /// <summary>
    /// Direction from the fire point to where an enemy will be when the projectile arrives, flat on the ground.
    /// Used by projectiles that fly in a straight line (Archer spread, Wizard fireball), which would otherwise miss a walking enemy.
    ///
    /// Time to arrive = distance / projectile speed. In that time the enemy walks Velocity x time further, so the tower aims at that spot.
    /// </summary>
    protected Vector3 FlatDirectionTo(EnemyMovement target)
    {
        Vector3 aimPoint = target.transform.position;

        float projectileSpeed = projectilePrefab != null ? projectilePrefab.Speed : 0f;
        if (projectileSpeed > 0f)
        {
            float distance = Vector3.Distance(firePoint.position, aimPoint);
            float travelTime = distance / projectileSpeed;
            aimPoint += target.Velocity * travelTime;
        }

        Vector3 direction = aimPoint - firePoint.position;
        direction.y = 0f;
        if (direction.sqrMagnitude < 0.001f)
        {
            direction = transform.forward;
            direction.y = 0f;
        }
        return direction.normalized;
    }
}
