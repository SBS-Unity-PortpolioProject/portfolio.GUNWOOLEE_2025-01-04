using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

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
            _animator.SetBool(AnimationStrings.HasTarget, value);
        }
    }
    
    public bool CanMove => _animator.GetBool(AnimationStrings.CanMove);
    
    public bool IsAlive => _animator.GetBool(AnimationStrings.IsAlive);
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private Damageable _damageable;
    
    Transform nextWaypoint;
    
    private int waypointIndex = 0;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = _wayPoints[waypointIndex];
    }

    private void Update()
    {
        HasTarget = _attackDetectionZone.DetectionColliders.Count > 0;  
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
        
        _rb.velocity = directionToWaypoint * _speed;
        
        UpdateDirection();
        
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        if (distance < wayPointDistance)
        {
            waypointIndex++;
            
            if(waypointIndex >= _wayPoints.Count) waypointIndex = 0;
            
            nextWaypoint = _wayPoints[waypointIndex];
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
        else
        {
            if (_rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}




