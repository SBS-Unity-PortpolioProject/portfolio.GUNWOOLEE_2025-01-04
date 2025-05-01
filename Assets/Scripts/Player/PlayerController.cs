using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using sceneManager = UnityEngine.SceneManagement.SceneManager;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] public float _WalkSpeed = 5f;
    
    [SerializeField] public float _RunSpeed = 10f;
    
    [SerializeField] private float _JumpImpulse = 3;
    
    [SerializeField] private GameObject _DashEffect;
    
    [SerializeField] private GameObject _settingScreen;
    
    [SerializeField] private DialogueUI _dialogueUI;
    
    [SerializeField] private DialogueActivator _dialogueActivator;
    
    [SerializeField] private SettingScreen _settingScreenUI;
    
    [SerializeField] bool _operator = true;
    
    public AudioSource _audioSource;

    [SerializeField] AudioClip[] _AudioClips;
    
    public Vector2 _DashImpulse = new Vector2(5f, 10f);
    
    public GameObject gmaeOverUI;
    
    private bool _cutScene = false;
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput = Vector2.zero;
    private Animator _animator;
    private TouchingDirection _touchingDirection;
    
    public float CurrentMoveSpeed
    {
        get
        {
            if (!_operator) return 0;
            
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
    
    private void Update()
    {
        if (!_IsAlive)
        {
            GameOver();
            return;
        }
 
        if (_dialogueUI._dialogueStarted || _settingScreenUI._isStarted || _cutScene)
        {
            _operator = false;
        }
        else
        {
            _operator = true;
        }
        
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
        
        if(!_IsMoving)
        {
            return;
        }

        if (_IsMoving && !IsRunning)
        {
            _audioSource.PlayOneShot(_AudioClips[0]);
        }
        else if (_IsMoving && IsRunning)
        {
            _audioSource.PlayOneShot(_AudioClips[1]);
        }
        else if (!_IsMoving || (!_IsMoving && !IsRunning))
        {
            _audioSource.PlayOneShot(null);
        }
        
        _animator.SetFloat(AnimationStrings.YVelocity, _rb.velocity.y);
    }
    
    public void OnMoveInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive || !_operator) return;
        
        _moveInput = context.ReadValue<Vector2>();
        _audioSource.Play();
        
        IsMoving = (_moveInput != Vector2.zero);
        
        OnFacingDirection(_moveInput);
    }

    public void OnFacingDirection(Vector2 Input)
    {
        if (!_IsAlive || !_operator) return;
        
        if (Input.x < 0) transform.localScale = new Vector3(-3, 3, 1);
        
        else if (Input.x > 0) transform.localScale = new Vector3(3, 3, 1);
    }
    
    public void OnRunInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive || !_operator) return;
        
        if (context.started)
        {
            //_audioSource.Play(); 달리기 소리
            IsRunning = true;
        }

        if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJumpInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive || !_operator) return;
        
        if (context.started && _touchingDirection._isGround)
        { 
            //_audioClips.PlayOneShot(); 점프 소리
            _rb.velocity = new Vector2(_rb.velocity.x, _JumpImpulse);
            _animator.SetTrigger(AnimationStrings.IsJump);
        }
    }
    
    public void OnAttackInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive || !_operator) return;
        
        if (context.started && _touchingDirection._isGround)
        {
            //_audioClips.PlayOneShot(); 공격 소리
            _animator.SetTrigger(AnimationStrings.Attack);
        }
    }

    private IEnumerator ApplyAttackVelocity()
    {
        float attackDuration = 0.17f;
        float attackSpeed = 15f;

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
        
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }
    
    public void OnDashInputAction()
    {
        if (!_IsAlive || !_operator) return;
        
        StartCoroutine(ApplyAttackVelocity());
    }

    public void OnDashAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive || !_operator) return;
        
        if (context.started && _touchingDirection._isGround)
        {
            //_audioClips.PlayOneShot(); 슈퍼점프 소리

            _DashEffect.SetActive(true);
            
            Vector3 mouseScreenPos = Input.mousePosition;
            
            mouseScreenPos.z = 10f; 
            
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
               
            float direction = (mouseWorldPos.x > transform.position.x) ? 1f : -1f;
            
            _rb.velocity = Vector2.zero;
                
            _rb.velocity = new Vector2(_DashImpulse.x * direction, _DashImpulse.y);
        }
    }
    
    public void OnDashFlipInputAction()
    {
        if (!_IsAlive || !_operator) return;

        if (_rb.transform.localScale.x > 0)
        {
            _rb.transform.localScale = new Vector3(-3, 3, 1);
        }
        else if (_rb.transform.localScale.x < 0)
        {
            _rb.transform.localScale = new Vector3(3, 3, 1);
        }
    }

    public void EnableControls()
    {
        _cutScene = false;
        GetComponent<Rigidbody2D>().simulated = true;
    }

    public void DisableControls()
    {
        _cutScene = true;
        GetComponent<Rigidbody2D>().simulated = false;
    }
    
    public void OnEscapeInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _settingScreen.SetActive(true);
        }
    }

    public void OnKnockback(Vector2 Knockback)
    {
        _rb.velocity = new Vector2(Knockback.x, _rb.velocity.y +  Knockback.y);
    }

    void GameOver()
    {
        if(gmaeOverUI != null)
        {
            gmaeOverUI.SetActive(true);
        }
    }
}