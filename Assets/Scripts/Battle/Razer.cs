using UnityEngine;

public class Razer : MonoBehaviour
{
    [SerializeField] private BattleDetectionZone _battleDetection;

    Animator _animator;
    
    private bool _operator = false;

    private bool _Operator
    {
        get {return _operator; }
        set
        {
            _operator = value;
            
            _animator.SetBool(AnimationStrings.Operation, value);
        }
    }
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
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
