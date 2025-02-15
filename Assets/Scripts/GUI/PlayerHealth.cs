using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] Slider _playerHealthSlider;
    
    Damageable _playerDamageable;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!player) Debug.LogError("Player not found");
        _playerDamageable = player.GetComponent<Damageable>();
        
        _playerDamageable._onHealthChanged.AddListener(OnHealthChanged);
        
        OnHealthChanged(_playerDamageable.Health, _playerDamageable.MaxHealth);
    }

    private void OnDestroy()
    {
        _playerDamageable._onHealthChanged.RemoveListener(OnHealthChanged);
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
