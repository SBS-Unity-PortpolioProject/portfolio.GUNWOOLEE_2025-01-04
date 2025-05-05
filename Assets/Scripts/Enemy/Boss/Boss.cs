using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject _warning;
    [SerializeField] private GameObject _bossEffect1;
    [SerializeField] private GameObject _bossEffect2;
    [SerializeField] private GameObject _bossEffect3;
    [SerializeField] private GameObject _swordEffect1;
    [SerializeField] private GameObject _swordEffect2;
    [SerializeField] private GameObject _swordEffect3;
    [SerializeField] private GameObject _swordEffect4;
    [SerializeField] private GameObject _dialogueActivator;
    [SerializeField] private AudioSource _walkSound;
    [SerializeField] AudioClip[] _audioClips;
    public AudioSource _audioSource;
    
    public float speed;
    public float attackMoveSpeed;
    public float fallSpeed;
    public bool _start = false;

    private bool Move = true;
    private int _attackCount = 0;
    private bool _attacked = false;
    
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

    private bool _stop = false;
    
    public bool Stop
    {
        get { return _stop; }
        private set
        {
            _stop = value;
            _animator.SetBool(AnimationStrings.Stop, value);
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
    Vector3 _playerPosition = Vector3.zero;
    Vector3 _originPosition = new Vector3(0, 1, 0);
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _touchingDirection = GetComponent<TouchingDirection>();
        _audioSource = GetComponent<AudioSource>();
        
        StartCoroutine(AttackCool());
        _damageable._isInvincible = true;
    }


    private void Update()
    {
        if (!_isAlive)
        {
            StartCoroutine(Death());
        }

        if (Move)
        {
            Movement();    
        }
        
        if (_canAttack && _attacked)
        {
            int _randomAttack = Random.Range(0, 2);
            int _rnadomSkill = Random.Range(0, 2);

            if (_randomAttack == 0 && _attackCount < 3)
            {
                StartCoroutine(Attacking1());
                _attackCount++;
            }
            else if (_randomAttack == 1 && _attackCount < 3)
            {
                Routine = true;
                StartCoroutine(Attacking2());
                _attackCount++;
            }
            else if (_rnadomSkill == 0)
            {
                if (_attackCount == 3)
                {
                    _attackCount = 0;
                    StartCoroutine(Attacking3());
                }
            }
            else if (_rnadomSkill == 1)
            {
                if (_attackCount == 4)
                {
                    _attackCount = 0;
                    StartCoroutine(Attacking3());
                }
            }
        }
    }

    private IEnumerator AttackCool()
    {
        float timer = Random.Range(3f, 6f);
        yield return new WaitForSeconds(timer);
        _damageable._isInvincible = true;
        CanAttack = true;
        _attacked = true;
    }
    
    private IEnumerator Attacking1()
    {
        Move = false;
        IsMoving = false;
        _attacked = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger(AnimationStrings.Attack);
        _player.transform.position = new Vector3(0, -1.37f, 0);
        
        Vector3 RightPosition = new Vector3(7, -1.86f, 0);
        Vector3 LeftPosition = new Vector3(-11, -1.86f, 0);
        
        int Rocation = Random.Range(0, 2);
        
        Wall.SetActive(true);
        yield return Vanish();
        
        if (Rocation == 0)
        {
            transform.position = RightPosition;
            transform.localScale = new Vector3(-8, 8, 1);
            
            yield return Appear();
            
            while (Mathf.Abs(transform.position.x - LeftPosition.x) > 0.01f)
            {
                _moveDirection = (LeftPosition - transform.position).normalized;
                _moveDirection.y = 0;
                _rb.velocity = _moveDirection * attackMoveSpeed;
                
                yield return null;
            }
            
            CanAttack = false;
            _animator.ResetTrigger(AnimationStrings.Attack);
            
            yield return Vanish();

            Wall.SetActive(false);
            
            transform.position = _originPosition;
            
            yield return Appear();
            
            StartCoroutine(AttackCool());
            Move = true;
        }
        else if (Rocation == 1)
        {
            transform.position = LeftPosition;
            transform.localScale = new Vector3(8, 8, 1);
            
            yield return Appear();
            
            while (Mathf.Abs(transform.position.x - RightPosition.x) > 0.01f)
            {
                _moveDirection = (RightPosition - transform.position).normalized;
                _moveDirection.y = 0;
                _rb.velocity = _moveDirection * attackMoveSpeed;
                
                yield return null;
            }
            
            CanAttack = false;
            _animator.ResetTrigger(AnimationStrings.Attack);
            
            Wall.SetActive(false);
            
            yield return Vanish();
            
            transform.position = _originPosition;
            
            yield return Appear();
            
            StartCoroutine(AttackCool());
            Move = true;
        }
    }   
    
    private IEnumerator Attacking2()
    {
        Move = false;
        IsMoving = false;
        _attacked = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger(AnimationStrings.Attack2);
        
        int _randomRoutine = Random.Range(1, 3);
        Vector3 _movePosition = Vector3.zero;
        
        yield return Vanish();
        
        yield return Warning();
        
        yield return WaitAttack2();
        
        yield return Vanish();
        transform.position = _originPosition;
        
        yield return Appear();

        if (_randomRoutine != 0 && Routine)
        {
            for (int i = 0; i <= _randomRoutine; i++)
            {
                yield return Vanish();
        
                yield return Warning();
                
                yield return WaitAttack2();
        
                yield return Vanish();
                transform.position = _originPosition;
        
                yield return Appear();
            }
            CanAttack = false;
            Routine = false;
        }
        _animator.ResetTrigger(AnimationStrings.Attack2);
        StartCoroutine(AttackCool());
        Move = true;
    }

    private IEnumerator Attacking3()
    {
        Move = false;
        IsMoving = false;
        _attacked = false;
        Stop = false;
        _rb.velocity = Vector2.zero;
        _damageable._isInvincible = true;
        _animator.SetTrigger(AnimationStrings.Attack3);

        yield return Vanish();
        transform.position = _originPosition;
        
        yield return Appear();
        
        yield return Attack3();

        yield return ChargingAttack();

        yield return WaitAttack3();
        
        yield return Vanish();
        transform.position = _originPosition;
        
        CanAttack = false;
        Stop = false;
        _animator.ResetTrigger(AnimationStrings.Attack3);
        StartCoroutine(AttackCool());
        Move = true; 
    }
    private void Movement()
    {
        _moveDirection = (_playerController.transform.position - transform.position).normalized;
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

    private void PlayerPositionCheck()
    {
        _playerPosition = _playerController.transform.position;
    }

    public void VanishSound()
    { 
        _audioSource.PlayOneShot(_audioClips[0]);
    }

    public void AppearSound()
    {
        _audioSource.PlayOneShot(_audioClips[1]);
    }

    public void MoveAttackSound()
    {
        _audioSource.PlayOneShot(_audioClips[2]);
    }
    
    private IEnumerator Vanish()
    {
        _animator.SetTrigger(AnimationStrings.Vanish);
        yield return new WaitForSeconds(0.5f);
        _animator.ResetTrigger(AnimationStrings.Vanish);
    }

    private IEnumerator Appear()
    {
        _animator.SetTrigger(AnimationStrings.Appear);
        yield return new WaitForSeconds(0.75f);
        _animator.ResetTrigger(AnimationStrings.Appear);
    }

    private IEnumerator WaitAttack2()
    {
        yield return new WaitForSeconds(1.5f);
        _animator.ResetTrigger(AnimationStrings.Attack2);
    }

    private IEnumerator Warning()
    {
        PlayerPositionCheck();
        transform.position = new Vector3(_playerPosition.x, _playerPosition.y - 0.5f, 0);
        _warning.transform.position = _playerPosition;
        _warning.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(0.7f);
        
        _warning.gameObject.SetActive(false);
        _animator.SetTrigger(AnimationStrings.Attack2);
        _audioSource.PlayOneShot(_audioClips[3]);
        yield return new WaitForSeconds(0.2f);
        _audioSource.PlayOneShot(_audioClips[3]);
    }

    private IEnumerator Attack3()
    {
        _moveDirection = (new Vector3(0, 1.9f, 0) - transform.position).normalized;
        _moveDirection.x = 0;
        
        yield return new WaitForSeconds(0.4f); // 0.4초
        while (transform.position.y > -1.9f)
        {
            _rb.velocity = _moveDirection * fallSpeed;
            
            yield return null;
        }
        _audioSource.PlayOneShot(_audioClips[4]);
        
        yield return new WaitForSeconds(1f);
        _audioSource.PlayOneShot(_audioClips[5]);
        _swordEffect1.SetActive(true);
        _swordEffect2.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _audioSource.PlayOneShot(_audioClips[5]);
        _swordEffect3.SetActive(true);
        _swordEffect4.SetActive(true);
        
        yield return new WaitForSeconds(0.84f);
        _bossEffect1.SetActive(true);
        _bossEffect2.SetActive(true);
        yield return new WaitForSeconds(0.41f);
    }

    private IEnumerator ChargingAttack()
    {
        float count = 0.5f;
        _bossEffect3.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(1.6f);
        
        _swordEffect1.SetActive(false);
        _swordEffect2.SetActive(false);
        _swordEffect3.SetActive(false);
        _swordEffect4.SetActive(false);
        _bossEffect1.SetActive(false);
        _bossEffect2.SetActive(false);
        _bossEffect3.SetActive(true);
        
        while (_bossEffect3.transform.localScale.x < 6)
        {
            _bossEffect3.transform.localScale = new Vector3(1 + count, 1 + count, 1);
            count++;
            
            yield return new WaitForSeconds(0.01f);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        _bossEffect3.SetActive(false);
    }
    
    private IEnumerator WaitAttack3()
    {
        _damageable._isInvincible = false;
        yield return new WaitForSeconds(7f); // 원래 7초
        Stop = true; // Vanish로 가기 위한 조건
        yield return new WaitForSeconds(1.1f);
        _damageable._isInvincible = true;
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);
        _dialogueActivator.SetActive(true);
        Destroy(gameObject);
    }
}















