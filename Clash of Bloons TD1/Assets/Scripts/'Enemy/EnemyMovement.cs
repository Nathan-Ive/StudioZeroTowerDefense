using System.Collections.Generic; // Needed for List
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public static readonly List<EnemyMovement> ActiveEnemies = new List<EnemyMovement>();

    [Header("Waypoint Instellingen")]
    public Transform[] waypoints;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private int currentWaypointIndex = 0;
    private bool hasReachedEnd = false;
    private EnemyDamageScript damageScript;

    // Slow and stun, applied by the Medieval Hooligan.
    private float slowMultiplier = 1f; // 1 = normal speed, 0.5 = half speed
    private float slowTimer = 0f;
    private float stunTimer = 0f;

    void OnEnable()
    {
        ActiveEnemies.Add(this);
    }

    void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }

    void Start()
    {
        damageScript = GetComponent<EnemyDamageScript>();
    }

    void Update()
    {
        if (hasReachedEnd || waypoints.Length == 0) return;

        // A stunned enemy does not move.
        UpdateSlowAndStun();
        if (stunTimer > 0f) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint == null)
        {
            hasReachedEnd = true;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * slowMultiplier * Time.deltaTime); // slowMultiplier is 1 when not slowed
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (currentWaypointIndex >= waypoints.Length - 1)
            {
                hasReachedEnd = true;
                if (damageScript != null)
                {
                    GameObject fenceObject = GameObject.FindWithTag("Fence");
                    damageScript.ReachedEnd(fenceObject);
                }
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }


    /// <summary>
    /// Distance this enemy still has to walk: to its next waypoint, plus every path
    /// segment after that. Small = almost at the fence. Used for "First" and "Last" targeting.
    /// </summary>
    public float DistanceToEnd
    {
        get
        {
            if (hasReachedEnd || waypoints == null || waypoints.Length == 0) return 0f;

            float distance = 0f;

            Transform nextWaypoint = waypoints[currentWaypointIndex];
            if (nextWaypoint != null)
            {
                distance += Vector3.Distance(transform.position, nextWaypoint.position);
            }

            for (int i = currentWaypointIndex; i < waypoints.Length - 1; i++)
            {
                if (waypoints[i] != null && waypoints[i + 1] != null)
                {
                    distance += Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
                }
            }

            return distance;
        }
    }

    /// <summary>
    /// Direction and speed this enemy is walking right now (zero when stunned or finished).
    /// Straight projectiles use it to aim where the enemy will be.
    /// </summary>
    public Vector3 Velocity
    {
        get
        {
            if (hasReachedEnd || stunTimer > 0f || waypoints == null || waypoints.Length == 0) return Vector3.zero;

            Transform nextWaypoint = waypoints[currentWaypointIndex];
            if (nextWaypoint == null) return Vector3.zero;

            Vector3 direction = (nextWaypoint.position - transform.position).normalized;
            return direction * speed * slowMultiplier;
        }
    }

    /// <summary>
    /// Slow this enemy down. multiplier 0.5 = half speed. duration in seconds (Mathf.Infinity = permanent).
    /// When two slows overlap, the stronger one wins.
    /// </summary>
    public void ApplySlow(float multiplier, float duration)
    {
        if (slowTimer <= 0f || multiplier <= slowMultiplier)
        {
            slowMultiplier = Mathf.Clamp01(multiplier);
            slowTimer = Mathf.Max(slowTimer, duration);
        }
    }

    /// <summary>Stop this enemy completely for a number of seconds.</summary>
    public void Stun(float duration)
    {
        stunTimer = Mathf.Max(stunTimer, duration);
    }

    private void UpdateSlowAndStun()
    {
        if (slowTimer > 0f)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                slowMultiplier = 1f; // Slow down wears off
            }
        }

        if (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
        }
    }
}
