using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private Animator _animator;
    private bool _isInvincible;
    [SerializeField] private float invincibilityTime = 0.25f;
    private float _timeSinceHit;

    [SerializeField] private int maxHealth = 100;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    [SerializeField] private int health = 100;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField] private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            _isAlive = value;
            _animator.SetBool(AnimationStrings.IsAlive, value);
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInvincible)
        {
            if (_timeSinceHit >= invincibilityTime)
            {
                _isInvincible = false;
                _timeSinceHit = 0f; 
            }
            _timeSinceHit += Time.deltaTime;
        }
        //Hit(10);
    }

    public void Hit(int damage)
    {
        if (IsAlive && !_isInvincible)
        {
            Health -= damage;
            _isInvincible = true;
        }
    }
}