using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    
    private CapsuleCollider2D _touchingCol;
    private Animator _animator;
    private RaycastHit2D[] _groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] _wallHits = new RaycastHit2D[5];
    private RaycastHit2D[] _ceilingHits = new RaycastHit2D[5];
    private Vector2 WallCheckDirection => transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    [SerializeField]
    private bool _isGrounded;

    public bool IsGrounded
    {
        get => _isGrounded;
        private set
        {
            _isGrounded = value;
            _animator.SetBool(AnimationStrings.IsGrounded, value);
        }
    }
    
    [SerializeField]
    private bool _isOnWall;

    public bool IsOnWall
    {
        get => _isOnWall;
        private set
        {
            _isOnWall = value;
            _animator.SetBool(AnimationStrings.IsOnWall, value);
        }
    }
    
    [SerializeField]
    private bool _isOnCeiling;

    public bool IsOnCeiling
    {
        get => _isOnCeiling;
        private set
        {
            _isOnCeiling = value;
            _animator.SetBool(AnimationStrings.IsOnCeiling, value);
        }
    }

    private void Awake()
    {
        _touchingCol = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        IsGrounded =  _touchingCol.Cast(Vector2.down, castFilter, _groundHits, groundDistance) > 0;
        IsOnWall = _touchingCol.Cast(WallCheckDirection, castFilter, _wallHits, wallDistance) > 0;
        IsOnCeiling = _touchingCol.Cast(Vector2.up, castFilter, _ceilingHits, ceilingDistance) > 0;
    }
}
