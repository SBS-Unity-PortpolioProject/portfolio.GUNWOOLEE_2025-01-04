using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private PlayerController Player;
    
    public float speed;

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
    
    private bool _isMoving = true;

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
    private Vector2 _moveDirection = Vector2.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _touchingDirection = GetComponent<TouchingDirection>();

        StartCoroutine(AttackCool());
    }


    private void Update()
    {
        if (!_isAlive)
        {
            return;
        }

        if (!_isMoving)
        {
            return;
        }
        
        Movement();
        
        if (_canAttack)
        {
            int _randomAttack = Random.Range(0, 2);
            
            if (_randomAttack == 0)
            {
                Attacking1();
            }
            else
            {
                Attacking2();
            }
        }
    }

    private IEnumerator AttackCool()
    {
        float timer = Random.Range(3f, 6f);
        yield return new WaitForSeconds(timer);
        _canAttack = true;
    }
    
    private void Attacking1()
    {
        int Rocation = Random.Range(0, 2);
        
        if (Rocation == 0)
        {
            transform.position = new Vector2(0,0);// 오른쪽 끝
            transform.localScale = new Vector3(-6, 6, 1);            
        }
        else if (Rocation == 1)
        {
            transform.position = new Vector2(0,0);// 왼쪽 끝
            transform.localScale = new Vector3(6, 6, 1);
        }
        
        bool _routine = false;
        _attack = true;
        StartCoroutine(AttackCool());
        _rb.transform.position = new Vector3(0, 5, 0);
    }
    
    private void Attacking2()
    {
        bool _routine = false;
        _attack2 = true;
        StartCoroutine(AttackCool());
        _rb.transform.position = new Vector3(0, 5, 0);
    }
    private void Movement()
    {
        _moveDirection = (Player.transform.position - transform.position).normalized;
        _moveDirection.y = 0;
        UpdateDirection();
        _rb.velocity = _moveDirection * speed;
        _isMoving = _rb.velocity != Vector2.zero;
    }

    private void UpdateDirection()
    {
        if (transform.localScale.x > 0)
        {
            if (_rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (transform.localScale.x < 0)
        {
            if (_rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1f * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}























