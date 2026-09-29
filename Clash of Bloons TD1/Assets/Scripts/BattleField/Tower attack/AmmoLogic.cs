using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage")]
    [Tooltip("damageAmount")]
    public float damageAmount = 10f;

    [Header("tag filter")]
    [Tooltip("If it needs to do damage based on the other tag X it here.")]
    public bool useTagFilter = false;

    [Tooltip("the damage it needs to do.")]
    public string targetTag = "Enemy";
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
            else
            {
               
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