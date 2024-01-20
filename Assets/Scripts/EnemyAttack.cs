using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float timeToDamage = 1f;

    private float _damageTime;
    private bool _isDamage = true;

    void Start()
    {
        _damageTime = timeToDamage;
    }

    void Update()
    {
        if (!_isDamage)
        {
            _damageTime -= Time.deltaTime;

            if (_damageTime <= 0)
            {
                _isDamage = true;
                 _damageTime = timeToDamage;
            }
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth) && _isDamage)
        {
            playerHealth.ReduceHealth(damage);
            _isDamage = false;
        }
    }
}
