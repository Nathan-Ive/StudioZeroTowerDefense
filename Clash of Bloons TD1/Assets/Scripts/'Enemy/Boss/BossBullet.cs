using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Header("Bullet settings")]
    public float speed = 15f;
    public float damage = 25f;
    public float maxLifetime = 5f;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        transform.LookAt(target.position);
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            TowerHealth towerHealth = target.GetComponent<TowerHealth>();
            if (towerHealth != null)
            {
                towerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}