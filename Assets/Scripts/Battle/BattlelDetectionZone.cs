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
    [SerializeField] public int _count = 1;
    
    private bool PlayerDirection = false;
    
    bool _operation = false;
    
    public bool Operation { get { return _operation; } set { _operation = value; } }

    public void Update()
    {
        if (_detectionObjects.Count == 0)
        {
            _count += 1;
        }
    }

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
