using System.Collections.Generic;
using UnityEngine;

public class BattleDetectionZone : MonoBehaviour
{
    [SerializeField] private List<GameObject> _detectionObjects = new List<GameObject>();
    [SerializeField] public int _wave = 0;
    [SerializeField] private int _waveObjecctCount1;
    [SerializeField] private int _waveObjecctCount2;

    private bool playerDirection = false;
    private bool _operation = false;
    private bool _waveIncremented = false;
    
    public bool Operation { get { return _operation; } }

    private void Update()
    {
        if (!_waveIncremented && _detectionObjects.Count == _waveObjecctCount1 || _detectionObjects.Count == _waveObjecctCount2 && playerDirection)
        {
            _wave += 1;
            _operation = true;
            _waveIncremented = true;
        }

        if (_detectionObjects.Count == _waveObjecctCount2)
        {
            _waveIncremented = false;
        }
        
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerDirection && _detectionObjects.Count > 1)
        {
            playerDirection = true;
            _operation = true;
            _detectionObjects.Add(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") && !_detectionObjects.Contains(collision.gameObject))
        {
            _detectionObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerDirection = false;
            _detectionObjects.Remove(collision.gameObject);
        }
        else
        {
            _detectionObjects.Remove(collision.gameObject);
        }

        if (_detectionObjects.Count <= 1)
        {
            _operation = false;
        }
    }
}