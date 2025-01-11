using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private DialogueUI _dialogueUI;
    
    public DialogueUI DialogueUI => _dialogueUI;

    public float CurrentSpeed
    {
        get
        {
            if (_dialogueUI.IsOpen) return 0;
            
            return _walkSpeed;
        }
    }
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput = Vector2.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_dialogueUI.IsOpen) _rb.velocity = new Vector2(0, 0);
        
        _rb.velocity = new Vector2(_moveInput.x * CurrentSpeed, _rb.velocity.y);
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
         _moveInput = context.ReadValue<Vector2>();
    }
    
    
}























