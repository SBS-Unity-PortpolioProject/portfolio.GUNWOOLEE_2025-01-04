using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBarChange : MonoBehaviour
{
    [SerializeField] private GameObject _BreakPlayerBar;
    [SerializeField] private DialogueActivator _dialogueActivator;

    private void Update()
    {
        if (_dialogueActivator._change2)
        {
            _BreakPlayerBar.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
