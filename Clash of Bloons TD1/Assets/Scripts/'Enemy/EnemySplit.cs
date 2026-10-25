using Unity.VisualScripting;
using UnityEngine;

public class EnemySplit : MonoBehaviour
{
    [SerializeField] private GameObject splitEnemy;
    [SerializeField] private int SplitAmount = 2;
    [SerializeField] private float splitTime = 1f;

    private void Update()
    {
        splitTime -= Time.deltaTime;

        if (splitTime <= 0)
        {
            for (int i = 0; i < SplitAmount; i++)
            { 
                Instantiate(splitEnemy, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
