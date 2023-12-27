using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float walkDistance = 6f;
    [SerializeField]
    private float walkSpeed = 1f;
    [SerializeField]
    private float timeToWait = 5f;

    private Rigidbody2D _rb;
    private Vector2 _leftBoundaryPosition;
    private Vector2 _rightBoundaryPosition;
    private bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight;
    }
    private bool _isWait = false;
    private float _waitTime;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _leftBoundaryPosition = transform.position;
        _rightBoundaryPosition = _leftBoundaryPosition + Vector2.right * walkDistance;
        _waitTime = timeToWait;
    }

    void Update()
    {
        if (_isWait)
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
        Vector2 nextPoint = Vector2.right * walkSpeed * Time.fixedDeltaTime;

        if (!_isFacingRight)
        {
            nextPoint.x *= -1;
        }

        if (!_isWait)
        {
            _rb.MovePosition((Vector2)transform.position + nextPoint);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_leftBoundaryPosition, _rightBoundaryPosition);
    }

    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = transform.localScale;

        playerScale.x *= -1;

        transform.localScale = playerScale;
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
