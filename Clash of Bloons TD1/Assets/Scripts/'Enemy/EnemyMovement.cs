using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Waypoint Instellingen")]
    public Transform[] waypoints;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private int currentWaypointIndex = 0;
    private bool hasReachedEnd = false;
    private EnemyDamageScript damageScript;
    void Start()
    {
        damageScript = GetComponent<EnemyDamageScript>();
    }
    void Update()
    {
        if (hasReachedEnd || waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
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
                    damageScript.ReachedEnd(targetWaypoint.gameObject);
                }
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }
}