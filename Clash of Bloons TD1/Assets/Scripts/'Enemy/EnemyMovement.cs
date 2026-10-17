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

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint == null)
        {
            hasReachedEnd = true;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
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
}
