using UnityEngine;
using UnityEngine.UI;

public class NukeDamage : MonoBehaviour
{
    [Header("BananaNukeSettings")]
    public int damageAmount = 999;
    public float range = 999f;
    private Image BananaNuke;

    private void OnCollisionEnter(Collision collision)
    {
        DealDamageInRange();
        Destroy(gameObject);
    }

    void InstantiateNewObject()
    {
        GameObject cube = new GameObject("BananaNuke");

        Instantiate(cube, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    private void DealDamageInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                TargetHealth targetHealth = hitCollider.GetComponent<TargetHealth>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damageAmount);
                }
                Destroy(hitCollider.gameObject);
            }
        }
    }

}
