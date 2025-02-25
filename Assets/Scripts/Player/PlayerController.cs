using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float _WalkSpeed = 5f;
    
    [SerializeField] private float _RunSpeed = 10f;
    
    [SerializeField] private float _JumpImpulse = 3;
    
    [SerializeField] private GameObject _DashEffect;
    
    [SerializeField] private GameObject _settingScreen;
    
    [SerializeField] private DialogueUI _dialogueUI;
    
    [SerializeField] private SettingScreen _settingScreenUI;
    
    [SerializeField] bool _operator = true;
    
    public Vector2 _DashImpulse = new Vector2(5f, 10f);

    public GameObject gmaeOverUI;
    
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
        
        if (_dialogueUI.dialogueStarted || _settingScreenUI._isStarted)
        {
            _operator = false;
        }
        else
        {
            _operator = true;
        }
        
        if(!_IsMoving) return;
        
        _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);
        _animator.SetFloat(AnimationStrings.YVelocity, _rb.velocity.y);
    }
    
    
    public void OnMoveInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive || !_operator) return;
        
         _moveInput = context.ReadValue<Vector2>();
         
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
            _rb.velocity = new Vector2(_rb.velocity.x, _JumpImpulse);
            _animator.SetTrigger(AnimationStrings.IsJump);
        }
    }
    
    public void OnAttackInputAction(InputAction.CallbackContext context)
    {
        if (!_IsAlive || !_operator) return;
        
        if (context.started && _touchingDirection._isGround)
        {
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
        _operator = true;
    }

    public void DisableControls()
    {
        _operator = false;
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




