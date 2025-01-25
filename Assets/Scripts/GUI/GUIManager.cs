using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GUIManager : MonoBehaviour
{
    public static UnityAction<Vector3, int> characterDamaged;
    
    public static UnityAction<Vector3, int> characterHealed;
    
    [SerializeField] GameObject DamageTextPrefab;
    [SerializeField] GameObject HealTextPrefab;
    
    private Canvas gameCanvas;

    private void Awake()
    {
        gameCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        characterDamaged += OnCharacterDamaged;
        characterHealed += OnCharacterHealed;
    }

    private void OnDisable()
    {
        characterDamaged -= OnCharacterDamaged;
        characterHealed -= OnCharacterHealed;
    }

    private void OnCharacterDamaged(Vector3 position, int damage)
    {
        Vector3 spawnPoint = Camera.main.WorldToScreenPoint(position);
        
        GameObject obj = Instantiate(DamageTextPrefab, spawnPoint, Quaternion.identity, gameCanvas.transform);
        
        TextMeshProUGUI Text = obj.GetComponent<TextMeshProUGUI>();
        Text.text = damage.ToString();
    }

    private void OnCharacterHealed(Vector3 position, int heal)
    {
        Vector3 spawnPoint = Camera.main.WorldToScreenPoint(position);
        
        GameObject obj = Instantiate(HealTextPrefab, spawnPoint, Quaternion.identity, gameCanvas.transform);
            
        TextMeshProUGUI Text = obj.GetComponent<TextMeshProUGUI>();
        Text.text = heal.ToString();
    }

    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
#if UNITY_STANDALONE
            Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif            
        }
    }
}
