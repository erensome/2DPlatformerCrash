using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float airWalkSpeed = 3f;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    private Rigidbody2D _rb;
    private TouchingDirections _touchingDirections;
    private Animator _animator;
    private Vector2 _moveInput;
    
    private float CurrentMoveSpeed
    {
        get
        {
            // prevents walking on the wall.
            if (IsMoving && !_touchingDirections.IsOnWall)
            {
                if (_touchingDirections.IsGrounded) // Ground Move
                {       
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    // Just walking - Not running
                    return walkSpeed;
                }
                else
                {
                    // Air Move
                    return airWalkSpeed;
                }
            }
            // Idle Speed - Not Moving
            return 0;
        }
    }
    
    [SerializeField]
    private bool _isMoving;
    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            _animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }
    
    [SerializeField]
    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            _animator.SetBool(AnimationStrings.IsRunning, value);
        }
    }

    private bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            // Flip the x-axis
            transform.localScale *= new Vector2(-1f, 1f);
            _isFacingRight = value;
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirections = GetComponent<TouchingDirections>();
    }

    private void FixedUpdate()
    {
        // Gelen input değerini direkt olarak velocity'e set ettiğimiz için fixedDeltaTime kullanımı gerekli değildir.
        // !Bug - W veya S tuşlarına basıldığı zaman _moveInput.x değeri değişiyor bu da hızın azalmasına sebep oluyor.
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
        
        _animator.SetFloat(AnimationStrings.YVelocity, _rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        // Move the character when the player pressed A or D keys.
        if (_moveInput.y == 0f)
        {
            IsMoving = _moveInput != Vector2.zero;
            SetFacingDirection(_moveInput);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // Facing right
            IsFacingRight = true;
        }
        else if (_moveInput.x < 0 && IsFacingRight)
        {
            // Facing left
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && _touchingDirections.IsGrounded)
        {
            _animator.SetTrigger(AnimationStrings.Jump);
            _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
        }
    }
}
