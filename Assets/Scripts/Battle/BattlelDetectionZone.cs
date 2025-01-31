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
    
    private bool PlayerDirection = false;
    
    bool _operation = false;
    
    public bool Operation { get { return _operation; } set { _operation = value; } }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !PlayerDirection)
        {
            PlayerDirection = true;
            
            Operation = true;
            
            _detectionObjects.Add(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") && !_detectionObjects.Contains(collision.gameObject))
        {
            _detectionObjects.Add(collision.gameObject);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            _detectionObjects.Remove(collision.gameObject);
        }
        
        if (_detectionObjects.Count == 1 && PlayerDirection)
        {
            Operation = false;
        }
    }
}
