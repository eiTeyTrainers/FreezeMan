using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayMovement : MonoBehaviour
{
    public InputControls CustomInput = null;
    private float _moveVector;
    public float Speed = 5f;

    private void OnEnable()
    {
        CustomInput.Enable();
        CustomInput.Player.Move.performed += Move;
        CustomInput.Player.Move.canceled += StopMove;
    }

    private void StopMove(InputAction.CallbackContext obj)
    {
        _moveVector = 0;
    }

    private void OnDisable()
    {
        CustomInput.Player.Move.performed -= Move;
    }

    private void Move(InputAction.CallbackContext callbackContext)
    {
        _moveVector = callbackContext.ReadValue<Axis>();

    }
    

    void FixedUpdate()
    {
        

    }

    void Awake()
    {
        CustomInput = new InputControls();

    }
}