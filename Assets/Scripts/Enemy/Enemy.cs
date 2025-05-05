using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum EWalkableDirection
{
    Right,
    Left,
}


public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    [SerializeField] private DetectionZone _playerDetectionZone;
    [SerializeField] private DetectionZone _attackDetectionZone;
    [SerializeField] private GameObject _my;
    [SerializeField] private float wayPointDistance = 0.1f;
    
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();
    
    [SerializeField] private AudioSource _walkAudioSource;
    [SerializeField] private AudioSource _runAudioSource;
    [SerializeField] private AudioClip[] _audioClips;
    public AudioSource _audioSource;
    
    private bool _hasTarget = false;
    
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
        }
    }
    
    private bool _canAttack = false;

    public bool CanAttack
    {
        get { return _canAttack; }
        private set
        {
            _canAttack = value;
            // _audioClips.PlatOneShot(); 공격하는 소리
            _animator.SetBool(AnimationStrings.CanAttack, value);
        }
    }
    
    private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving;}
        set
        {
            _isMoving = value;
            _animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }
    
    private bool _isOnCliff = false;

    public bool IsOnCliff
    {
        get { return _isOnCliff; }
        private set
        {
            _isOnCliff = value;
        }
    }

    public float CurrentSpeed
    {
        get
        {
            if(HasTarget) return 4f;
 
            if (!IsMoving) return 0;
            return _speed;
        }
    }
    
    private EWalkableDirection _walkableDirection = EWalkableDirection.Right;

    public EWalkableDirection WalkableDirection
    {
        get { return _walkableDirection; }
        private set
        {
            if (value == EWalkableDirection.Right)
            {
                moveDirection = Vector2.left;
            }
            else if (value == EWalkableDirection.Left)
            {
                moveDirection = Vector2.right;
            }
        }
    }
    
    public bool IsAlive => _animator.GetBool(AnimationStrings.IsAlive);
    
    private bool _wallCheck = false;
    private bool _attackCheck = false;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private Damageable _damageable;
    private TouchingDirection _touchingDirection;
    private Vector2 moveDirection = Vector2.right;
    
    public ContactFilter2D contactFilter;
    Transform nextWaypoint;
    [SerializeField] private int waypointIndex = 0;
    CapsuleCollider2D _TouchingCollider;
    public float CliffDistence = 1f;
    public Vector2 _cliffdirection = new Vector2(1f,-1f);
    RaycastHit2D[] _isCliffDetection = new RaycastHit2D[5];
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _touchingDirection = GetComponent<TouchingDirection>();
        _TouchingCollider = GetComponent<CapsuleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        nextWaypoint = _wayPoints[waypointIndex];
    }

    private void FixedUpdate()
    {
        if (!IsAlive)
        {
            Destroy(_my);
            return;
        }
        
        if (_attackCheck)
        {
            _rb.velocity = Vector2.zero;
            return;
        }
        
        HasTarget = _playerDetectionZone.DetectionColliders.Count > 0;
        IsMoving = true;
        
        if (IsOnCliff && HasTarget)
        {
            _rb.velocity = Vector2.zero;
            return;
        }
        
        if (IsOnCliff && !_touchingDirection._isGround)
        {
            return;
        }
        
        if (_attackDetectionZone.DetectionColliders.Count > 0 && !_attackCheck)
        {
            _attackCheck = true;
            StartCoroutine(Attack());
        }
        else
        {
            CanAttack = false;
        }
        
        Move();
        
        Debug.DrawRay(_TouchingCollider.bounds.center, _cliffdirection.normalized * CliffDistence, Color.red);
        _TouchingCollider.Cast(_cliffdirection, contactFilter, _isCliffDetection, CliffDistence);

        IsOnCliff = !Physics2D.Raycast(_TouchingCollider.bounds.center,
            _cliffdirection.normalized * CliffDistence, CliffDistence, LayerMask.GetMask("Ground"));
        
        if (IsOnCliff)
        {
            FlipDirection();
            waypointIndex++;
            
            if (waypointIndex >= _wayPoints.Count)
            {
                waypointIndex = 0;
            }
            
            nextWaypoint = _wayPoints[waypointIndex];
        }
    }
    
    private void Move()
    {
        if (!IsAlive) return;
        
        if(IsMoving == false) return;
        
        _walkAudioSource.Play();
        
        Vector2 direction = GetMoveDirection();
        direction.y = 0;
        _rb.velocity = direction * CurrentSpeed;
        
        UpdateDirection();
                    
        if (_touchingDirection.IsOnWall && !_wallCheck)
        {
            {
                StartCoroutine(WallCheck());
                FlipDirection();
                waypointIndex++;
                if (waypointIndex >= _wayPoints.Count) waypointIndex = 0;
                nextWaypoint = _wayPoints[waypointIndex];
            }
        } 
    }
    
    private Vector2 GetMoveDirection()
    {
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        
        if (distance < wayPointDistance)
        {
            waypointIndex++;
            
            if(waypointIndex >= _wayPoints.Count) waypointIndex = 0;
            nextWaypoint = _wayPoints[waypointIndex];
        }
        
        if (HasTarget)
        {
            foreach (var target in _playerDetectionZone.DetectionColliders)
            {
                PlayerController player = target.GetComponent<PlayerController>();
                if (player != null)
                {
                    _walkAudioSource.Stop();
                    _runAudioSource.Play();
                    return (player.transform.position - transform.position).normalized;
                }
            }
            return (nextWaypoint.position - transform.position).normalized;
        }
        else
        {
            return (nextWaypoint.position - transform.position).normalized;
        }
    }

    private void UpdateDirection()
    {
        if (transform.localScale.x > 0)
        {
            if (_rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                _cliffdirection = new Vector2(-1f, -1f);
            }
        }
        else if (transform.localScale.x < 0)
        {
            if (_rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                _cliffdirection = new Vector2(1f, -1f);
            }
        }
    }

    private void FlipDirection()
    {
        if (_walkableDirection == EWalkableDirection.Right)
        {
            _walkableDirection = EWalkableDirection.Left;
        }
        else if (_walkableDirection == EWalkableDirection.Left)
        {
            _walkableDirection = EWalkableDirection.Right;
        }
    }

    public void OnHit(Vector2 Knockback)
    {
        _rb.velocity = new Vector2(Knockback.x, _rb.velocity.y);

        if (Knockback.x > 0 && transform.localScale.x > 0) FlipDirection();
        else if (Knockback.x < 0 && transform.localScale.x < 0) FlipDirection();
    }

    private IEnumerator WallCheck()
    {
        _wallCheck = true;
        yield return new WaitForSeconds(0.1f);
        _wallCheck = false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        _audioSource.PlayOneShot(_audioClips[0]);
        CanAttack = true;
        _attackCheck = false;
    }
}