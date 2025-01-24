using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float _WalkSpeed = 5f;
    
    [SerializeField] private float _RunSpeed = 10f;
    
    [SerializeField] private float _JumpImpulse = 3;
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput = Vector2.zero;
    private Animator _animator;
    private TouchingDirection _touchingDirection;

    public float CurrentMoveSpeed
    {
        get
        {
            if(!IsMoving) return 0;
            
            if(!_CanMove) return 0;
            
            if(_touchingDirection.IsOnWall) return 0;
            
            if(IsRunning) return _RunSpeed;
            
            return _WalkSpeed;
        }
    }
    
    private bool _IsMoving = false;
    public bool IsMoving
    {
        get { return _IsMoving;}
        set
        {
            _IsMoving = value;
            
            _animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }
    
    private bool _IsRunning = false;

    public bool IsRunning
    {
        get { return _IsRunning; }
        
        set
        {
            _IsRunning = value;
            
            _animator.SetBool(AnimationStrings.IsRunning, value);
        }
    }

    public bool _CanMove => _animator.GetBool(AnimationStrings.CanMove);
    
    public bool _IsAlive => _animator.GetBool(AnimationStrings.IsAlive);
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirection = GetComponent<TouchingDirection>();
    }
    
    private void FixedUpdate()
    {
        if(!_IsMoving)
            return;
        
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
        _animator.SetFloat(AnimationStrings.YVelocity, _rb.velocity.y);
    }
    
    
    public void OnMoveInputAction(InputAction.CallbackContext context)
    {
         _moveInput = context.ReadValue<Vector2>();
         
         IsMoving = (_moveInput != Vector2.zero);
         
         OnFacingDirection(_moveInput);
    }

    public void OnFacingDirection(Vector2 Input)
    {
        if(!_IsAlive)
            return;
        if (Input.x < 0) transform.localScale = new Vector3(-3, 3, 1);
        
        else if (Input.x > 0) transform.localScale = new Vector3(3, 3, 1);
    }
    
    public void OnRunInputAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }

        if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJumpInputAction(InputAction.CallbackContext context)
    {
        if (context.started && _touchingDirection._isGround)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _JumpImpulse);
            _animator.SetTrigger(AnimationStrings.IsJump);
        }
    }

    public void OnAttackInputAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _animator.SetTrigger(AnimationStrings.Attack);
        }
    }
    
}























