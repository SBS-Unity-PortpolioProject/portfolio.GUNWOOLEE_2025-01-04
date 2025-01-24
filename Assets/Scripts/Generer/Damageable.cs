using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    
    [SerializeField] private int _maxHealth = 100;

    public UnityEvent<int, int> _onHealthChanged;
    
    public int Health => _health;
    
    public int MaxHealth => _maxHealth;

    public bool GetHit(int damage)
    {
        _health -= damage;
        
        return true;
    }
    
    
    
}
