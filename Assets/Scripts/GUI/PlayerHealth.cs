using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Slider _playerHealthSlider;
    [SerializeField] TextMeshProUGUI _playerHealthText;
    
    Damageable _playerDamageable;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;
        _playerDamageable = player.GetComponent<Damageable>();
        
        _playerDamageable._onHealthChanged.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(int newhealth, int maxhealth)
    {
        _playerHealthSlider.value = CalculrateSliderValue(newhealth, maxhealth);
        
    }

    private float CalculrateSliderValue(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }
    
}
