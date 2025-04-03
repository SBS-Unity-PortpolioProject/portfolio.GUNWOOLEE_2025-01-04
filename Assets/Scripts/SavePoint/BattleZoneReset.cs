using UnityEngine;

public class BattleZoneReset : MonoBehaviour
{ 
    [SerializeField] private GameObject _battleZonePrefab;
    [SerializeField] private FallRespawn _fallRespawn;
    [SerializeField] private BattleDialogue _battleDialogue;
    private GameObject currentBattleZone;

    private void Start()
    {
        SpawnBattleZone();
    }

    private void Update()
    {
        if (_fallRespawn._battleZoneReset)
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
        currentBattleZone = Instantiate(_battleZonePrefab, _battleZonePrefab.transform.position, Quaternion.identity);
        
        if (_battleDialogue != null)
        {
            BattleDetectionZone zone = currentBattleZone.GetComponentInChildren<BattleDetectionZone>();
            _battleDialogue._battleDetectionZone = zone;    
        }
    }
}
