using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float walkDistance = 6f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float chasingSpeed = 3f;
    [SerializeField] private float timeToWait = 5f;
    [SerializeField] private float timeToChaise = 3f;
    [SerializeField] private float minDistanceToPlayer = 1.5f;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _leftBoundaryPosition;
    private Vector2 _rightBoundaryPosition;
    private bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight;
    }
    private bool _isWait = false;
    private bool _isChasingPlayer = false;
    private float _walkSpeed;
    private float _waitTime;
    private float _chaiseTime;
    private Vector2 _nextPoint;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _leftBoundaryPosition = transform.position;
        _rightBoundaryPosition = _leftBoundaryPosition + Vector2.right * walkDistance;
        _waitTime = timeToWait;
        _chaiseTime = timeToChaise;
        _walkSpeed = patrolSpeed;
    }

    void Update()
    {
        if (_isChasingPlayer)
        {
            StartChasingTimer();
        }

        if (_isWait && !_isChasingPlayer)
        {
            StartWaitTimer();
        }

        if (ShouldWait())
        {
            _isWait = true;
        }
    }

    void FixedUpdate()
    {
        _nextPoint = Vector2.right * _walkSpeed * Time.fixedDeltaTime;

        if (_isChasingPlayer && Mathf.Abs(DistanceToPlayer()) < minDistanceToPlayer)
        {
            return;
        }

        if (_isChasingPlayer)
        {
            ChasePlayer();
        }
        else if (!_isWait)
        {
            Patrol();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_leftBoundaryPosition, _rightBoundaryPosition);
    }

    public void Patrol()
    {
        if (!_isFacingRight)
        {
            _nextPoint.x *= -1;
        }

        _rb.MovePosition((Vector2)transform.position + _nextPoint);
    }

    public void ChasePlayer()
    {
        float distance = DistanceToPlayer();

        if (distance < 0)
        {
            _nextPoint.x *= -1;
        }


        if ((distance < 0.2f && _isFacingRight) || (distance > 0.2f && !_isFacingRight))
        {
            Flip();
        }

        _rb.MovePosition((Vector2)transform.position + _nextPoint);
    }

    public void StartChasingPlayer()
    {
        _walkSpeed = chasingSpeed;
        _chaiseTime = timeToChaise;
        _isChasingPlayer = true;
    }

    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = transform.localScale;

        playerScale.x *= -1;

        transform.localScale = playerScale;
    }

    private float DistanceToPlayer()
    {
        return _playerTransform.position.x - transform.position.x;
    }

    private bool ShouldWait()
    {
        bool isOutOfRightBoundary = _isFacingRight && transform.position.x >= _rightBoundaryPosition.x;
        bool isOutOfLeftBoundary = !_isFacingRight && transform.position.x <= _leftBoundaryPosition.x;

        return isOutOfRightBoundary || isOutOfLeftBoundary;
    }

    private void StartWaitTimer()
    {
        _waitTime -= Time.deltaTime;

        if (_waitTime <= 0f)
        {
            _waitTime = timeToWait;
            _isWait = false;
            Flip();
        }
    }

    private void StartChasingTimer()
    {
        _chaiseTime -= Time.deltaTime;

        if (_chaiseTime <= 0f)
        {
            _walkSpeed = patrolSpeed;
            _chaiseTime = timeToChaise;
            _isChasingPlayer = false;
        }
    }
}
