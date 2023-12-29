using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float walkDistance = 6f;
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float timeToWait = 5f;
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
    private float _waitTime;
    private Vector2 _nextPoint;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _leftBoundaryPosition = transform.position;
        _rightBoundaryPosition = _leftBoundaryPosition + Vector2.right * walkDistance;
        _waitTime = timeToWait;
    }

    void Update()
    {
        if (_isWait && !_isChasingPlayer)
        {
            Wait();
        }

        if (ShouldWait())
        {
            _isWait = true;
        }
    }

    void FixedUpdate()
    {
        _nextPoint = Vector2.right * walkSpeed * Time.fixedDeltaTime;

        if (Mathf.Abs(DistanceToPlayer()) < minDistanceToPlayer)
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

    private void Wait()
    {
        _waitTime -= Time.deltaTime;

        if (_waitTime <= 0f)
        {
            _waitTime = timeToWait;
            _isWait = false;
            Flip();
        }
    }
}
