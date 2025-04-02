using UnityEngine;

public class BattleDialogue : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueActivator;
    
    public BattleDetectionZone _battleDetectionZone;
    
    public bool _battleClear = false;
    
    private void Update()
    {
        if (_battleDetectionZone._battleClear)
        {
            _battleClear = true;
            _dialogueActivator.SetActive(true);
            // _battleClear = false;
        }
    }
}
