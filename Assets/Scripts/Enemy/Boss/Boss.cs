using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    
    public float speed = 5f;

    private bool _canAttack = false;

    public bool CanAttack
    {
        get { return _canAttack;}
        private set
        {
            _canAttack = value;
            _animator.SetBool(AnimationStrings.CanAttack, value);
        }
    }
    
    private bool _attack = false;

    public bool Attack
    {
        get { return _attack;}
        private set
        {
            _attack = value;
            _animator.SetTrigger(AnimationStrings.Attack);
        }
    }
    
    private bool _attack2 = false;

    public bool Attack2
    {
        get { return _attack2; }
        private set
        {
            _attack2 = value;
            _animator.SetTrigger(AnimationStrings.Attack2);
        }
    }
    
    private bool _isMoving = false;

    public bool IsMoving
    {
        get { return _isMoving;}
        private set
        {
            _isMoving = value;
            _animator.SetBool(AnimationStrings.IsMoving, value);
        }
    } 
    
    private bool _isAlive = true;

    public bool IsAlive
    {
        get { return _isAlive; }
        private set
        {
            _isAlive = value;
            _animator.SetBool(AnimationStrings.IsAlive, value);
        }
    }

    private Rigidbody2D _rb;
    private Animator _animator;
    private Damageable _damageable;
    private TouchingDirection _touchingDirection;
    
    
    private int _randomAttack = Random.Range(0, 1);
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _touchingDirection = GetComponent<TouchingDirection>();
    }

    private void Update()
    {
        if (_canAttack)
        {
            if (_randomAttack == 0)
            {
                _attack = true;
            }
            else
            {
                _attack2 = true;
            }
        }
    }
}
