using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleDetectionZone : MonoBehaviour
{
    [SerializeField] private List<GameObject> _detectionObjects = new List<GameObject>();
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dialogueActivator;
    [SerializeField] public int _wave = 0;
    [SerializeField] private int _waveObjectCount1;
    [SerializeField] private int _waveObjectCount2;
    [SerializeField] private int _waveObjectCount3;
    [SerializeField] private bool _waveIncremented = false;
    [SerializeField] private bool _playerDirection = false;
    
    public bool _end = false;
    
    private bool _operation = false;
    
    public bool Operation { get { return _operation; } }

    private void Update()
    {
        if (!_waveIncremented && (_detectionObjects.Count == _waveObjectCount1 || _detectionObjects.Count == _waveObjectCount2 || _detectionObjects.Count == _waveObjectCount3) && _playerDirection)
        {
            _wave += 1;
            _operation = true;
            _waveIncremented = true;
        }

        if (_wave == 1 && _detectionObjects.Count == _waveObjectCount2)
        {
            _waveIncremented = false;
        }

        if (_wave == 2 && _detectionObjects.Count == _waveObjectCount3)
        {
            _waveIncremented = false;
        }

        if (_detectionObjects.Count == 1 && dialogueActivator != null)
        {
            player.transform.position = new Vector2(341.52f, 4.7f);
            dialogueActivator.SetActive(true);
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_playerDirection && _detectionObjects.Count > 1)
        {
            _playerDirection = true;
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
            _playerDirection = false;
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