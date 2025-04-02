using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZoneReset : MonoBehaviour
{ 
    [SerializeField] private GameObject battleZonePrefab;
    [SerializeField] private FallRespawn fallRespawn;
    [SerializeField] private DialogueActivator dialogueActivator;
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
        currentBattleZone = Instantiate(battleZonePrefab, battleZonePrefab.transform.position, Quaternion.identity);
        BattleDetectionZone zone = currentBattleZone.GetComponentInChildren<BattleDetectionZone>();
        dialogueActivator._battleDetectionZone = zone;
    }
}
