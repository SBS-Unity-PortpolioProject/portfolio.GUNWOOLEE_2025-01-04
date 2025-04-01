using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZoneReset : MonoBehaviour
{ 
    [SerializeField] private GameObject battleZonePrefab;
    [SerializeField] private FallRespawn fallRespawn;
    private GameObject currentBattleZone;

    private void Start()
    {
        SpawnBattleZone();
    }

    private void Update()
    {
        if (fallRespawn._battleZoneReset)
        {
            ResetBattleZone();
        }
    }
    
    private void ResetBattleZone()
    {
        if (currentBattleZone != null)
        {
            Destroy(currentBattleZone);
        }
        
        SpawnBattleZone();
    }

    private void SpawnBattleZone()
    {
        currentBattleZone = Instantiate(battleZonePrefab, new Vector3(118.3351f, 9.629911f, 0), Quaternion.identity);
    }
}
