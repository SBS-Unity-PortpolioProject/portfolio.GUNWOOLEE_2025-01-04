using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.Rendering;

public class BattleDetectionZone : MonoBehaviour
{
    [SerializeField] private List<GameObject> _detectionObjects = new List<GameObject>();
    
    private Animator _animator;
    
    private static bool _operation = false;

    public class Utility
    {
        public static bool OperationManager(bool operation)
        {
            _operation = operation;
            return _operation;
        }
        
        public static bool GetOperationStatus()
        {
            return _operation;
        }
    }
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            bool operationResult = Utility.OperationManager(true);
            
            _detectionObjects.Add(collision.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        _detectionObjects.Remove(collision.gameObject);
        
        if (_detectionObjects.Count <= 0)
        {
            bool operationResult = Utility.OperationManager(false);
            
        }
    }
    
    
}
