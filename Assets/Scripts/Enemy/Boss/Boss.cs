using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private PlayerController Player;
    [SerializeField] private GameObject Wall;
    
    public float speed;
    public float attackMoveSpeed;

    private bool _canAttack = false;
    private bool _next = false;
    private bool Move = true;

    public bool CanAttack
    {
        get { return _canAttack;}
        private set
        {
            _canAttack = value;
            _animator.SetBool(AnimationStrings.CanAttack, value);
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
    
    private bool _routine = true;

    public bool Routine
    {
        get { return _routine; }
        private set
        {
            _routine = value;
            _animator.SetBool(AnimationStrings.Routine, value);
        }
    }
    
    public bool _isAlive => _animator.GetBool(AnimationStrings.IsAlive);

    private Rigidbody2D _rb;
    private Animator _animator;
    private Damageable _damageable;
    private TouchingDirection _touchingDirection;
    private Vector2 _moveDirection = Vector2.zero;
    
    private bool _attacked = false;
    
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

        if (Move)
        {
            Movement();    
        }
        
        if (_canAttack && _attacked)
        {
            int _randomAttack = Random.Range(1, 2); // 2번째 공격만
            
            if (_randomAttack == 0)
            {
                StartCoroutine(Attacking1());
            }
            else if (_randomAttack == 1)
            {
                Routine = true;
                StartCoroutine(Attacking2());
            }
        }
    }

    private IEnumerator AttackCool()
    {
        float timer = Random.Range(3f, 6f);
        yield return new WaitForSeconds(timer); 
        CanAttack = true;
        _attacked = true;
    }
    
    private IEnumerator Attacking1()
    {
        Move = false;
        IsMoving = false;
        _attacked = false;
        _animator.SetTrigger(AnimationStrings.Attack); // Attack 키기
        _rb.velocity = Vector2.zero;
        
        Vector3 originPosition = new Vector3(0, 5, 0);
        Vector3 RightPosition = new Vector3(7, -1.86f, 0);
        Vector3 LeftPosition = new Vector3(-11, -1.86f, 0);
        
        int Rocation = Random.Range(0, 2);
        
        Wall.SetActive(true);
        yield return Vanish();
        
        if (Rocation == 0)
        {
            transform.position = RightPosition; // 오른쪽 끝
            transform.localScale = new Vector3(-6, 6, 1);
            
            yield return Appear();
            
            while (Mathf.Abs(transform.position.x - LeftPosition.x) > 0.01f)
            {
                _moveDirection = (LeftPosition - transform.position).normalized;
                _moveDirection.y = 0;
                _rb.velocity = _moveDirection * attackMoveSpeed;
                
                yield return null;
            }
            
            CanAttack = false;
            _animator.ResetTrigger(AnimationStrings.Attack); // Attack 끄기
            
            yield return Vanish();

            Wall.SetActive(false);
            
            transform.position = originPosition;
            
            yield return Appear();
            
            _attacked = false;
            StartCoroutine(AttackCool());
            Move = true;
        }
        else if (Rocation == 1)
        {
            transform.position = LeftPosition;// 왼쪽 끝
            transform.localScale = new Vector3(6, 6, 1);
            
            yield return Appear();
            
            while (Mathf.Abs(transform.position.x - RightPosition.x) > 0.01f)
            {
                _moveDirection = (RightPosition - transform.position).normalized;
                _moveDirection.y = 0;
                _rb.velocity = _moveDirection * attackMoveSpeed;
                
                yield return null;
            }
            
            CanAttack = false;
            _animator.ResetTrigger(AnimationStrings.Attack); // Attack 끄기
            
            Wall.SetActive(false);
            
            yield return Vanish();
            
            transform.position = originPosition;
            
            yield return Appear();
            
            _attacked = false;
            StartCoroutine(AttackCool());
            Move = true;
        }
    }   
    
    private IEnumerator Attacking2()
    {
        Move = false;
        IsMoving = false;
        _attacked = false;
        _animator.SetTrigger(AnimationStrings.Attack2);
        Vector3 _originPosition = new Vector3(0, 3, 0);
        Vector3 _playerPosition = Player.transform.position;
        int _randomRoutine = Random.Range(1, 3);

        yield return Vanish();
        transform.position = _playerPosition;

        yield return WaitAttack2();
        
        yield return Vanish();
        transform.position = _originPosition;
        
        yield return Appear();

        if (_randomRoutine != 0 && Routine)
        {
            for (int i = 0; i <= _randomRoutine; i++)
            {
                yield return Vanish();
                transform.position = _playerPosition;

                yield return WaitAttack2();
                
                yield return Vanish();
                transform.position = _originPosition;
                
                yield return Appear();
            }
            CanAttack = false;
            Routine = false;
        }
        _attacked = false;
        _animator.ResetTrigger(AnimationStrings.Attack2);
        StartCoroutine(AttackCool());
        Move = true;
    }
    private void Movement()
    {
        _moveDirection = (Player.transform.position - transform.position).normalized;
        _moveDirection.y = 0;
        UpdateDirection();
        _rb.velocity = _moveDirection * speed;

        if (Mathf.Abs(_moveDirection.x) > 0.05f)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
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

    private IEnumerator Vanish()
    {
        _animator.SetTrigger(AnimationStrings.Vanish);
        yield return new WaitForSeconds(0.7f);
        _animator.ResetTrigger(AnimationStrings.Vanish);
    }

    private IEnumerator Appear()
    {
        _animator.SetTrigger(AnimationStrings.Appear);
        yield return new WaitForSeconds(0.9f);
        _animator.ResetTrigger(AnimationStrings.Appear);
    }

    private IEnumerator WaitAttack2()
    {
        yield return new WaitForSeconds(1.5f);
    }
}















