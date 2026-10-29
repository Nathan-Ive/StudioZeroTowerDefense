using UnityEngine;

public class BossShoot : MonoBehaviour
{
    public float shootRange = 10f;
    public float fireRate = 1f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private float fireCooldown = 0f;

    void Update()
    {
        if (fireCooldown > 0f)
        {
            fireCooldown -= Time.deltaTime;
        }

        if (fireCooldown <= 0f)
        {
            Transform target = FindClosestTower();

            if (target != null)
            {
                Shoot(target);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    Transform FindClosestTower()
    {
        Transform closestTower = null;
        float shortestDistance = Mathf.Infinity;

        foreach (TowerHealth tower in TowerHealth.ActiveTowers)
        {
            if (tower == null) continue;

            float distanceToTarget = Vector3.Distance(transform.position, tower.transform.position);

            if (distanceToTarget <= shootRange && distanceToTarget < shortestDistance)
            {
                shortestDistance = distanceToTarget;
                closestTower = tower.transform;
            }
        }

        return closestTower;
    }

    void Shoot(Transform target)
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPosition = firePoint != null ? firePoint.position : transform.position;
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        BossBullet bulletScript = projectile.GetComponent<BossBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}