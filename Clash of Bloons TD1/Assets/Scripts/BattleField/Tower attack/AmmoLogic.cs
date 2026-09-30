using UnityEngine;

public class AmmoLogic : MonoBehaviour
{
    private Transform target;
    [Header("Movement")]
    public float speed = 20f;
    [Header("Damage")]
    public float damageAmount = 10f;
    [Header("Tag Filter")]
    public bool useTagFilter = false;
    public string targetTag = "Enemy";
    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            CheckAndDealDamage(target.gameObject);
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }
    private void OnCollisionEnter(Collision collision)
    {
        CheckAndDealDamage(collision.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        CheckAndDealDamage(other.gameObject);
    }
    private void CheckAndDealDamage(GameObject hitObject)
    {
        bool shouldDealDamage = false;

        if (useTagFilter)
        {
            if (hitObject.CompareTag(targetTag))
            {
                shouldDealDamage = true;
            }
        }
        else
        {
            if (hitObject.CompareTag("Enemy"))
            {
                shouldDealDamage = true;
            }
        }
        if (shouldDealDamage)
        {
            Health healthScript = hitObject.GetComponent<Health>();
            if (healthScript != null)
            {
                healthScript.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
        else
        {
            if (!hitObject.CompareTag("Bullet") && !hitObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}