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
    private Rigidbody2D _rigidbody2D;
    
    //Jump
    public float jumpForce;
    [SerializeField] float jumpStartTime;
    private float jumpTime;
    private bool isJumping;
    private bool isGrounded = true;
    void Awake()
    {
        CustomInput = new InputControls();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        CustomInput.Enable();
        CustomInput.Player.Move.performed += Move;    
        CustomInput.Player.Move.canceled += StopMove;
        CustomInput.Player.Freeze.performed += Freeze;
    }

    private void Freeze(InputAction.CallbackContext obj)
    {
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
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
        _moveVector = callbackContext.ReadValue<float>();
    }


    private void Update()
    {
        float oldY = _rigidbody2D.velocity.y;
        float x = _moveVector * Speed;
        _rigidbody2D.velocity = new Vector2(x, oldY);
    }
    private void Jump(){
        if(isGrounded == true)//add jupm input
        {
            isJumping = true;
            jumpTime = jumpStartTime;
            _rigidbody2D.velocity = Vector2.up * jumpForce;
        }
        if(jumpTime > 0){
            _rigidbody2D.velocity = Vector2.up * jumpForce;
            jumpTime -= Time.deltaTime;
        }else{
            isJumping = false;
        
        }

    if(Input.GetButtonUp("T"))
    {
        isJumping = false;
    }
    }
}