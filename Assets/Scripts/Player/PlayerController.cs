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

    [SerializeField] private int _attackCount;
    
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
    
    public float AttackCoolDown
    {
        get { return _animator.GetFloat(AnimationStrings.AttackCoolDown); }
        set { _animator.SetFloat(AnimationStrings.AttackCoolDown, value); }
    }
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirection = GetComponent<TouchingDirection>();
    }
    
    private void FixedUpdate()
    {
        if (!_IsAlive) return;
        
        if(!_IsMoving)
            return;
        
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
        _animator.SetFloat(AnimationStrings.YVelocity, _rb.velocity.y);
    }
    
    
    public void OnMoveInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive) return;
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
        if (!_IsAlive) return;
        
        if (context.started && _touchingDirection._isGround)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _JumpImpulse);
            _animator.SetTrigger(AnimationStrings.IsJump);
        }
    }

    public void OnAttackInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive) return;
        
        if (context.started && _touchingDirection._isGround)
        {
            _attackCount++;
            
            if (_attackCount == 1)
            {
                if (_rb.transform.localScale.x > 0)
                {
                    _animator.SetTrigger(AnimationStrings.Attack);
                    StartCoroutine(ApplyAttackVelocity());
                }  
                else if (_rb.transform.localScale.x < 0)
                {
                    _animator.SetTrigger(AnimationStrings.Attack);
                    StartCoroutine(ApplyAttackVelocity());
                }
            }
            else
            {
                _animator.SetTrigger(AnimationStrings.Attack);
            }

            if (_attackCount == 3 )
            {
                _attackCount = 0;
            }
        }
        
    }
    private IEnumerator ApplyAttackVelocity()
    {
        float attackDuration = 0.17f; // 이동 지속 시간
        float attackSpeed = 15f; // 빠르게 이동할 속도

        float timer = 0f;
        while (timer <= attackDuration)
        {
            if(_rb.transform.localScale.x > 0)
            {
                _rb.velocity = new Vector2(attackSpeed, _rb.velocity.y);
                timer += Time.deltaTime;
                yield return null;
            }
            else if (_rb.transform.localScale.x < 0)
            {
                _rb.velocity = new Vector2(-1 * attackSpeed, _rb.velocity.y);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        // 이동 종료 후 기본 속도로 복귀
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    public void OnKnockback(Vector2 Knockback)
    {
        _rb.velocity = new Vector2(Knockback.x, _rb.velocity.y +  Knockback.y);
    }
    
}







