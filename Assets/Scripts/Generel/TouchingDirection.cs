using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirection : MonoBehaviour
{
    [SerializeField] private DialogueUI _dialogueUI;
    [SerializeField] private PlayerController _player;
    
    public ContactFilter2D contactFilter;
    public float GroundDistence = 0.05f;
    public float WallDistence = 0.2f;

    private bool _IsGround = false;

    public bool _isGround
    {
        get{return _IsGround;}
        set
        {
            _IsGround = value;
            _Animator.SetBool(AnimationStrings.IsGround, value);
        }
    }
    
    private bool _IsOnWall = false;
    
    public bool IsOnWall
    {
        get {return _IsOnWall;}
        set
        {
            _IsOnWall = value;
        }
    }

    CapsuleCollider2D _TouchingCollider;
    Animator _Animator;
    
    RaycastHit2D[] GroundHits = new RaycastHit2D[5];
    RaycastHit2D[] WallHits = new RaycastHit2D[5];
    
    private Vector2 _WallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    
    private void Awake()    
    {
        _player = GetComponent<PlayerController>();
        _Animator = GetComponent<Animator>();
        _TouchingCollider = GetComponent<CapsuleCollider2D>();
    }
    
    void FixedUpdate()
    {
        if (_dialogueUI.IsOpen)
        {
            _player.IsMoving = false;
            _player.IsRunning = false;
            _isGround = true;
            return;
        }
        
        _isGround = _TouchingCollider.Cast(Vector2.down, contactFilter, GroundHits, GroundDistence) > 0;
        IsOnWall = _TouchingCollider.Cast(_WallCheckDirection, contactFilter, WallHits, WallDistence) > 0;
    }
}
