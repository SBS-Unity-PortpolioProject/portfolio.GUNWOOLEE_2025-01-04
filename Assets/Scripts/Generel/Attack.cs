using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int _attackDamage = 10;
    
    private void OnTriggerEnter2D (Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        
        if (damageable)
        {
            damageable.GetHit(_attackDamage);
        }   
        
    }
}
