using UnityEngine;
using static UnityEngine.GraphicsBuffer;


/// <summary>
/// This script tells towers where to aim and who to aim for.
/// </summary>
public class TowerAim
{

    private int _targetingMethod; // Placeholder Value (The idea is that the value points to one of three methods of aiming,
                                  // changing how the tower shoots based on the directions provided in the switch case.)

   
    
    
    //[SerializeField] private GameObject _target;
    //[SerializeField] private float _speed;


    //public void SetTarget(GameObject target)
    //{
    //    _target = target;
    //}

    //private void Update()
    //{
    //    float step = _speed * Time.deltaTime;
    //    transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
    //}




public void EnemyTarget()
    {
        switch (_targetingMethod)
        {
            case 0: // First Enemy

                break;

            case 1: // Last Enemy

                break;

            case 2: // Closest Enemy

                break;


        }
    }
}
