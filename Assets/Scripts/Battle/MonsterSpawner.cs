using System;
using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private BattleDetectionZone _battleDetection;
    [SerializeField] private GameObject monsterPrefab;
    // [SerializeField] private GameObject monsterPrefab2;
    // [SerializeField] private GameObject monsterPrefab3;
    // [SerializeField] private bool Spawn = false;
    // [SerializeField] private int _count = 1;
    private bool _spawned = false;
    private bool _operator = false;

    private bool _Operator
    {
        get { return _operator; }
        set
        {
            _operator = value;

            if (!_spawned && _battleDetection.Operation)
            {
                _spawned = true;
                Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            }
            
            // if (!_spawned && _battleDetection.Operation && _count == 1 && Spawn)
            // {
            //     _spawned = true;
            //     Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            // }
            // else return;
            // 
            // if (!_spawned && _battleDetection.Operation && _count == 2 && Spawn)
            // {
            //     _spawned = true;
            //     Instantiate(monsterPrefab2, transform.position, Quaternion.identity);
            // }
            // else return;
            // 
            // if (!_spawned && _battleDetection.Operation && _count == 3 && Spawn)
            // {
            //     _spawned = true;
            //     Instantiate(monsterPrefab3, transform.position, Quaternion.identity);
            // }
            // else return;
        }
    }

    private void Update()
    {
        // Spawn = _battleDetection._count == _count;
        
        if (_battleDetection.Operation)
        {
            _Operator = true;
            // _count += 1;
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
        else
        {
            _Operator = false;
        }
    }
    
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}