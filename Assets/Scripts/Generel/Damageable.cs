using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<Vector2> _onKnockback;
    
    public UnityEvent<int, int> _onHealthChanged;
    
    public UnityEvent _onDeath;
    
    [SerializeField] private int _health = 100;
    
    [SerializeField] private int _maxHealth = 100;
    
    [SerializeField] private bool _isAlive = true;
    
    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            
            if(! _isAlive) _onDeath.Invoke();
            _animator.SetBool(AnimationStrings.IsAlive, value);
        }
    }
        
    private Animator _animator;
        
    public int Health => _health;
    
    public int MaxHealth => _maxHealth;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public bool GetHit(int damage, Vector2 knockback)
    {
        if(_isAlive)
        {
            _health -= damage;

            GUIManager.characterDamaged.Invoke(transform.position, damage);
            
            _animator.SetTrigger(AnimationStrings.Hit);

            _onKnockback.Invoke(knockback);
            _onHealthChanged.Invoke(_health, _maxHealth);
            
            if (_health <= 0) IsAlive = false;
            
            return true;        
        }
        return true;
    }
    
    public bool GetHeal(int heal)
    {
        if(_isAlive)
        {
            int maxHeal = Mathf.Max(_maxHealth - _health, 0);

            int actualHeal = Mathf.Min(maxHeal, heal);

            _health += actualHeal;

            GUIManager.characterHealed.Invoke(transform.position, heal);

            _onHealthChanged.Invoke(_health, _maxHealth);
            
            return true;
        }
        return true;
    }
    
}