using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayMovement : MonoBehaviour
{
    public InputControls CustomInput = null;
    [SerializeField] public float moveVector;
    public float Speed = 5f;
    private Rigidbody2D _rigidbody2D;
    public float torqueFactor = -0.5f;
    
    //Jump
    public float jumpForce;
    public LayerMask whatIsGround;
    [SerializeField] float jumpStartTime;
    private float jumpTime;
    private bool isJumping;
    private bool isGrounded = true;

    private const float coyoteTime = 0.2f;

    private float coyoteTimeCounter;

    private const float jumpBufferTime = 0.2f;
    private float jumpBufferTimeCounter;
    
    //freeze 
    private GameObject[] Players;
    private Transform SpawnPosition;
    public bool isFrozen = false;
    public float lastMove = 0;

    void Awake()
    {
        CustomInput = new InputControls();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SpawnPosition = GameObject.Find("SpawnPoint").transform;
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
        CustomInput.Player.Jump.performed += JumpButtonPressed;
        CustomInput.Player.Jump.canceled += StopJumping;
    }

    private void Freeze(InputAction.CallbackContext obj)
    {
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        int newLayer = LayerMask.NameToLayer("Ground");
        gameObject.layer = newLayer;
        gameObject.transform.Find("Collider").gameObject.layer = newLayer;
        isFrozen = true;

        GameObject player = Players[Random.Range(0, Players.Length)];
        Instantiate(player, SpawnPosition.position, Quaternion.identity);
        CustomInput.Disable();
    }

    private void StopMove(InputAction.CallbackContext obj)
    {
        moveVector = 0;
    }

    private void OnDisable()
    {
        CustomInput.Player.Move.performed -= Move;
    }

    private void Move(InputAction.CallbackContext callbackContext)
    {
        moveVector = callbackContext.ReadValue<float>();
        lastMove = callbackContext.ReadValue<float>();
    }


    private void Update()
    {
        float jumpValue = CustomInput.Player.Jump.ReadValue<float>();
        if (jumpValue == 1 && isJumping)
        {
            if (jumpTime > 0)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        jumpBufferTimeCounter -= Time.deltaTime;
        
        if (coyoteTimeCounter > 0f && jumpBufferTimeCounter > 0f)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        float oldY = _rigidbody2D.velocity.y;
        float x = moveVector * Speed;
        if (!isGrounded)
        {
            _rigidbody2D.totalTorque = x * torqueFactor;
        }
        _rigidbody2D.velocity = new Vector2(x, oldY);
    }

    private void JumpButtonPressed(InputAction.CallbackContext obj)
    {
        jumpBufferTimeCounter = jumpBufferTime;
    }
    
    private void Jump()
    {
        Debug.Log("isGrounded: " + isGrounded + " coyote timer: " + coyoteTimeCounter + " jumpbuffer timer " + jumpBufferTimeCounter);
        _rigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); 

        isJumping = true;
        jumpTime = jumpStartTime;
        _rigidbody2D.velocity = Vector2.up * jumpForce;

        coyoteTimeCounter = 0f;
        jumpBufferTimeCounter = 0f;
    
    }

    private void StopJumping(InputAction.CallbackContext obj)
    {
        isJumping = false;
    }

    private bool IsCollisionGround(Collision2D other)
    {
        Vector2 surfaceNormal = other.contacts[0].normal;
        float rightAngle = Vector2.Angle(surfaceNormal, Vector2.right);
        if (rightAngle > 90)
        {
            rightAngle = 180 - rightAngle;
        }

        return rightAngle > 30;
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {   
        isGrounded = false;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(IsCollisionGround(other))
        {
            isGrounded = true;
        }
    }
}