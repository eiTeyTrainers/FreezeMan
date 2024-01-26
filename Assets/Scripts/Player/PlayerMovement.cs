using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayMovement : MonoBehaviour
{
    public InputControls CustomInput = null;
    private float _moveVector;
    public float Speed = 5f;
    private Rigidbody2D _rigidbody2D;
    
    //Jump
    public float jumpForce;
    public Transform feetPos;
    public LayerMask whatIsGround;
    [SerializeField] float jumpStartTime;
    [SerializeField]  float checkRadius;
    private float jumpTime;
    private bool isJumping;
    private bool isGrounded = true;
    
    //freeze 
    private GameObject[] Players;
    private Transform SpawnPosition;
    
    void Awake()
    {
        CustomInput = new InputControls();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SpawnPosition =  GameObject.Find("SpawnPoint").transform;
    }

    private void Start()
    {
        Players = Resources.LoadAll<GameObject>("Players");
        
    }

    private void OnEnable()
    {
        CustomInput.Enable();
        CustomInput.Player.Move.performed += Move;    
        CustomInput.Player.Move.canceled += StopMove;
        CustomInput.Player.Freeze.performed += Freeze;
        CustomInput.Player.Jump.performed += Jump;
        CustomInput.Player.Jump.canceled += StopJumping;
    }

    private void Freeze(InputAction.CallbackContext obj)
    {
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        int newLayer = LayerMask.NameToLayer("Ground");
        gameObject.layer = newLayer;


        GameObject player = Players[Random.Range(0, Players.Length)];
        Instantiate(player, SpawnPosition.position, Quaternion.identity);
        CustomInput.Disable();

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
        float jumpValue = CustomInput.Player.Jump.ReadValue<float>();
        if (jumpValue == 1 && isJumping)
        {
            if(jumpTime > 0){
                _rigidbody2D.velocity = Vector2.up * jumpForce;
                jumpTime -= Time.deltaTime;
            }else{
                isJumping = false;
            }
        }

        OnDrawGizmos();
    }

    private void FixedUpdate()
    {
        float oldY = _rigidbody2D.velocity.y;
        float x = _moveVector * Speed;
        _rigidbody2D.velocity = new Vector2(x, oldY);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius,whatIsGround);
        
        if(isGrounded)
        {
            _rigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); 

            isJumping = true;
            jumpTime = jumpStartTime;
            _rigidbody2D.velocity = Vector2.up * jumpForce;
        }
    
    }
    private void StopJumping(InputAction.CallbackContext obj)
    {
        isJumping = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(feetPos.position,checkRadius);
    }
}