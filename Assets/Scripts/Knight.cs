using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    private Rigidbody2D _rb;
    private TouchingDirections _touchingDirections;

    public enum WalkableDirection
    {
        Right,
        Left
    }

    private WalkableDirection _walkDirection;
    private Vector2 _walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get => _walkDirection;
        set
        {
            if (_walkDirection != value)
            {
                // Direction Flipped
                gameObject.transform.localScale *= new Vector2(-1f, 1f);
                if (value == WalkableDirection.Right)
                {
                    _walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    _walkDirectionVector = Vector2.left;
                }
            }

            _walkDirection = value;
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        if (_touchingDirections.IsGrounded && _touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        _rb.velocity = new Vector2(walkSpeed * _walkDirectionVector.x, _rb.velocity.y);
    }

    private void FlipDirection()
    {
        if (_walkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (_walkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Walk Direction is not set to right or left values.");
        }
    }
}