using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public class Razer : MonoBehaviour
{
    
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
        if (BattleDetectionZone.Utility.GetOperationStatus())
        {
            _Operator = true;
        }
        else
        {
            _Operator = false;
        }
    }
}
