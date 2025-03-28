using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revival : MonoBehaviour
{
    [SerializeField] private DeathSceneUI _deathSceneUI;
    
    public bool _revival = false;
    
    private int RevivalHealth = 100;
    
    private void Update()
    {
        if (_revival)
        {
            OnRevival();
            _revival = false;
        }
    }
    
    void OnRevival()
    {
        Damageable damageable = gameObject.GetComponent<Damageable>();
        damageable.GetHeal(RevivalHealth);
    }
}
