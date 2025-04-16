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
        
        Movement();
        
        if (_canAttack && _attacked)
        {
            int _randomAttack = Random.Range(0, 1); // 아직 2번째 완성 안되서 1번째 껏만
            
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
        CanAttack = true;
        _attacked = true;
    }
    
    private void Attacking1()
    {
        _attacked = false;
        Attack = true;
        
        int Rocation = Random.Range(0, 1);
        Vector3 originPosition = new Vector3(0, 5, 0);
        Vector3 RightPosition = new Vector3(8, -1.86f, 0);
        Vector3 LeftPosition = new Vector3(-12, -1.86f, 0);
        
        Wall.SetActive(true);
        StartCoroutine(WaitVanish());
        
        if (Rocation == 0 && _next)
        {
            Debug.Log("이동");
            transform.position = RightPosition;// 오른쪽 끝
            transform.localScale = new Vector3(-6, 6, 1);
            _animator.SetTrigger(AnimationStrings.Appear);
            
            //if (Appear)
            //{
            //    Debug.Log("공격 준비");
            //    Appear = false;
            //    StartCoroutine(WaitVanish());
            //
            //    while (Mathf.Abs(transform.position.x - RightPosition.x) > 0.01f)
            //    {
            //        Debug.Log("공격중");
            //        _moveDirection = (RightPosition - transform.position).normalized;
            //        _rb.velocity = _moveDirection * attackMoveSpeed;
            //        _isMoving = _rb.velocity != Vector2.zero;
            //    }
            //
            //    Attack = false;
            //    CanAttack = false;
            //    StartCoroutine(WaitVanish());
            //    Wall.SetActive(false);
            //
            //    if (Vanish)
            //    {
            //        Debug.Log("사라짐");
            //        transform.position = originPosition;
            //        Appear = true;
            //        Vanish = false;
            //    }
            //    Appear = false;
            //    StartCoroutine(AttackCool());
            //}
            
        }
        else if (Rocation == 1)
        {
            Debug.Log("이동");
            transform.position = LeftPosition;// 왼쪽 끝
            transform.localScale = new Vector3(6, 6, 1);
            _animator.SetTrigger(AnimationStrings.Appear);
            
            //if (Appear)
            //{
            //    Debug.Log("공격 준비");
            //    Appear = false;
            //    Attack = true;
            //    while (Mathf.Abs(transform.position.x - LeftPosition.x) > 0.01f)
            //    {
            //        Debug.Log("공격중");
            //        _moveDirection = (LeftPosition - transform.position).normalized;
            //        _rb.velocity = _moveDirection * attackMoveSpeed;
            //        _isMoving = _rb.velocity != Vector2.zero;
            //    }
            //    Attack = false;
            //    CanAttack = false; 
            //    Vanish = true;
            //    Wall.SetActive(false);
            //
            //    if (Vanish)
            //    {
            //        Debug.Log("사라짐");
            //        transform.position = originPosition;
            //        Appear = true;
            //        Vanish = false;
            //    }
            //    Appear = false;
            //    StartCoroutine(AttackCool());
            //}
        }
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
        IsMoving = _rb.velocity != Vector2.zero;
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

    private IEnumerator WaitVanish()
    {
        _animator.SetTrigger(AnimationStrings.Vanish);
        yield return new WaitForSeconds(0.2f);
        _next = true;
    }
}















