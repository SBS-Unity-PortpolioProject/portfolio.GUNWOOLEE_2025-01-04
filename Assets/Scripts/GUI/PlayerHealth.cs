using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Slider _playerHealthSlider;
    [SerializeField] Slider _bossHealthSlider;
    
    Damageable _playerDamageable;
    Damageable _bossDamageable;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        GameObject Boss = GameObject.FindGameObjectWithTag("Boss");
        
        _playerDamageable = player.GetComponent<Damageable>();
        
       if (_bossDamageable)
       {
           _bossDamageable = Boss.GetComponent<Damageable>();
       }
        
        if (_playerHealthSlider != null)
        {
            _playerDamageable._onHealthChanged.AddListener(OnHealthChanged);
            
            OnHealthChanged(_playerDamageable.Health, _playerDamageable.MaxHealth);
        }
        else if (_bossHealthSlider != null)
        {
            _bossDamageable._onHealthChanged.AddListener(OnHealthChanged);
            OnHealthChanged(_bossDamageable.Health, _bossDamageable.MaxHealth);

        }
    }

    private void OnDestroy()
    {
        if (_playerHealthSlider != null)
        {
            _playerDamageable._onHealthChanged.RemoveListener(OnHealthChanged);
        }
        else if (_bossHealthSlider != null)
        {
            _bossDamageable._onHealthChanged.RemoveListener(OnHealthChanged);
        }
    }

    private void OnHealthChanged(int newhealth, int maxhealth)
    {
        if (_playerHealthSlider != null)
        {
            _playerHealthSlider.value = CalculrateSliderValue(newhealth, maxhealth);   
        }
        else if (_bossHealthSlider != null)
        {
            _bossHealthSlider.value = CalculrateSliderValue(newhealth, maxhealth);
        }
    }

    private float CalculrateSliderValue(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }
}
