using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speedX = 1f;
    [SerializeField]
    private Animator animator;

    private Rigidbody2D _rb;
    private float _horizontal = 0f;
    private bool _isGround = false;
    private bool _isJump = false;
    private bool _isFacingRight = true;
    private bool _isFinish = false;
    private bool _isLeverArm = false;
    private Finish _finish;
    private LeverArm _leverArm;

    const float speedMultiplier = 50f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        _leverArm = FindObjectOfType<LeverArm>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");

        animator.SetFloat("speedX", Mathf.Abs(_horizontal));

        if (Input.GetKey(KeyCode.Space) && _isGround)
        {
            _isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isFinish)
            {
                _finish.FinishLevel();
            }

            if (_isLeverArm)
            {
                _leverArm.ActivateLevelArm();
            }
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = new Vector2(_horizontal * speedX * speedMultiplier * Time.fixedDeltaTime, _rb.velocity.y);

        if (_isJump)
        {
            _rb.AddForce(new Vector2(0f, 600f));
            _isGround = false;
            _isJump = false;
        }

        if (_horizontal > 0f && !_isFacingRight)
        {
            Flip();
        }
        else if (_horizontal < 0f && _isFacingRight)
        {
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LeverArm leverArmTmp = other.GetComponent<LeverArm>();

        if (other.CompareTag("Finish"))
        {
            _isFinish = true;
        }

        if (leverArmTmp != null)
        {
            _isLeverArm = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        LeverArm leverArmTmp = other.GetComponent<LeverArm>();

        if (other.CompareTag("Finish"))
        {
            _isFinish = false;
        }

        if (leverArmTmp != null)
        {
            _isLeverArm = false;
        }
    }

    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = transform.localScale;

        playerScale.x *= -1;

        transform.localScale = playerScale;
    }
}
