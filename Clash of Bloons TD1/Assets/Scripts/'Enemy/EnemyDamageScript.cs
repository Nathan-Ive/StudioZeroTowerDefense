using UnityEngine;

public class EnemyDamageScript : MonoBehaviour
{
    [Header("damage")]
    public int damageAmount = 1;

    private void OnCollisionEnter(Collision collision)
    {
        TryDamageTarget(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDamageTarget(other.gameObject);
    }

    void TryDamageTarget(GameObject target)
    {
        if (target.CompareTag("Fence"))
        {
            TargetHealth targetHealth = target.GetComponent<TargetHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
            }

            Debug.Log("damage to fence: " + target.name);
        }
    }
    public void ReachedEnd(GameObject endTarget)
    {
        if (endTarget != null)
        {
            TargetHealth targetHealth = endTarget.GetComponent<TargetHealth>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damageAmount);
            }

            Debug.Log("endpoint reached: " + endTarget.name);
        }

        Destroy(gameObject);
    }
}