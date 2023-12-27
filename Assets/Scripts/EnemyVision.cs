using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private GameObject currentHitObject;
    [SerializeField] private float circleRadius = 0.5f;
    [SerializeField] private float maxDistance = 3;
    [SerializeField] private LayerMask layerMask;

    private Vector2 _origin;
    private Vector2 _direction;
    private float _currentHitDistance;
    private EnemyController _enemyController;

    private void Start()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        _origin = transform.position;
        _direction = _enemyController.IsFacingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.CircleCast(_origin, _currentHitDistance, _direction, maxDistance, layerMask);

        if (hit)
        {
            currentHitObject = hit.transform.gameObject;
            _currentHitDistance = hit.distance;

            if (currentHitObject.CompareTag("Player"))
            {

            }
        }
        else
        {
            currentHitObject = null;
            _currentHitDistance = maxDistance;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_origin, _origin + _direction * _currentHitDistance);
        Gizmos.DrawWireSphere(_origin + _direction * _currentHitDistance, circleRadius);
    }
}
