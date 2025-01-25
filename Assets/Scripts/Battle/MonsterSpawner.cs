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
            if (!_spawned)
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
        }
        else
        {
            _Operator = false;
        }
    }
}