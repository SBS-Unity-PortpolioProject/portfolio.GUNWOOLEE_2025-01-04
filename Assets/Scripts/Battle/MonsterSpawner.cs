using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private BattleDetectionZone _battleDetection;
    [SerializeField] private GameObject monsterPrefab;
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
        }
    }

    private void Update()
    {
        if (_battleDetection.Operation)
        {
            _Operator = true;
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