using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float _walkSpeed = 5f;
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput = Vector2.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = new Vector2(_moveInput.x * _walkSpeed, _rb.velocity.y);
    }


    public void OnMoveInput(InputAction.CallbackContext context)
    {
         _moveInput = context.ReadValue<Vector2>();
    }
    
    
}























