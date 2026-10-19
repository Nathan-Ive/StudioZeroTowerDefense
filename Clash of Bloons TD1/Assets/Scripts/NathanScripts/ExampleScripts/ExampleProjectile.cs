using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ExampleProjectile : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _speed;



    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    private void Update()
    {
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
    }

}
