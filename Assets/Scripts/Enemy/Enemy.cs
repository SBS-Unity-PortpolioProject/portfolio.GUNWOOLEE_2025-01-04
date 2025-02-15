using System.Collections.Generic;
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
    [SerializeField] private float wayPointDistance = 0.1f;
    
    [SerializeField] private List<Transform> _wayPoints = new List<Transform>();
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
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private Damageable _damageable;
    private TouchingDirection _touchingDirection;
    private Vector2 moveDirection = Vector2.right;
    
    public ContactFilter2D contactFilter;
    Transform nextWaypoint;
    private int waypointIndex = 0;
    CapsuleCollider2D _TouchingCollider;
    public float CliffDistence = 1f;
    private Vector2 _cliffdirection = new Vector2(1f,-1f);
    RaycastHit2D[] _isCliffDetection = new RaycastHit2D[5];
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
        _touchingDirection = GetComponent<TouchingDirection>();
        _TouchingCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        nextWaypoint = _wayPoints[waypointIndex];
    }

    private void Update()
    {
        if (!IsAlive)
        {
            gameObject.SetActive(false);
            return;
        }
        
        IsMoving = true;
        HasTarget = _playerDetectionZone.DetectionColliders.Count > 0;
        CanAttack = _attackDetectionZone.DetectionColliders.Count > 0;
        Move();
        
        IsOnCliff = _TouchingCollider.Cast(_cliffdirection, contactFilter, _isCliffDetection, CliffDistence) < 1;

        if (IsOnCliff)
        {
            HasTarget = false;
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
        
        Vector2 direction = GetMoveDirection();
        direction.y = 0;
        _rb.velocity = direction * CurrentSpeed;
        
        UpdateDirection();
                    
        if (_touchingDirection.IsOnWall)
        {
            FlipDirection();
            waypointIndex++;
            if(waypointIndex >= _wayPoints.Count) waypointIndex = 0;
            nextWaypoint = _wayPoints[waypointIndex];
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
        else
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
}




