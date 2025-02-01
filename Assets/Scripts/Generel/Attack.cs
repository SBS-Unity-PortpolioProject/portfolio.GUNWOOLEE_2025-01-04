using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int _attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    private void OnTriggerEnter2D (Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        
        if (damageable)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ?
                knockback : new Vector2(-1 * knockback.x, knockback.y);
            
            damageable.GetHit(_attackDamage, deliveredKnockback);
        }   
        
    }
}
