using UnityEngine;

public class ExampleTower : MonoBehaviour
{

    public GameObject _targetTEMP;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _shootLocation;


    private void Start()
    {
        Shoot();
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(_projectile, _shootLocation.transform.position, Quaternion.identity, transform);
        ExampleProjectile projectileBehavior = projectile.GetComponent<ExampleProjectile>();
        projectileBehavior.SetTarget(_targetTEMP);
    }

}
