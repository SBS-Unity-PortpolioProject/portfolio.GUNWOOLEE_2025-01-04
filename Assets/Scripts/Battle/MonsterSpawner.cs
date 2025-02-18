using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private BattleDetectionZone _battleDetection;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject monsterPrefab2;
    [SerializeField] private GameObject monsterPrefab3;
    private bool _spawned = false;
    private int _lastWave = 0;

    private void Update()
    {
        if (_battleDetection._wave > _lastWave && _battleDetection.Operation)
        {
            _spawned = false;
            _lastWave = _battleDetection._wave;
            SpawnMonster(_battleDetection._wave);
        }
    }

    private void SpawnMonster(int wave)
    {
        if (_spawned) return;
        _spawned = true;

        GameObject monsterToSpawn = null;
        switch (wave)
        {
            case 1:
                monsterToSpawn = monsterPrefab;
                break;
            case 2:
                monsterToSpawn = monsterPrefab2;
                break;
            case 3:
                monsterToSpawn = monsterPrefab3;
                break;
        }

        if (monsterToSpawn != null)
        {
            Instantiate(monsterToSpawn, transform.position, Quaternion.identity);
            StartCoroutine(DestroyAfterDelay(0.5f));
        }
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}

