using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayMovement : MonoBehaviour
{
    public InputControls CustomInput = null;
    public float moveVector;
    public float Speed = 5f;
    public Rigidbody2D rigidbody2D;

    //Jump
    public float jumpForce;
    public LayerMask whatIsGround;
    [SerializeField] float jumpStartTime;
    private float jumpTime;
    private bool isJumping;
    private bool isGrounded = true;

    //freeze 
    private GameObject[] Players;
    private Transform SpawnPosition;
    public bool isFrozen = false;
    public float lastMove = 0;

    void Awake()
    {
        CustomInput = new InputControls();
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        CustomInput.Player.Jump.performed += Jump;
        CustomInput.Player.Jump.canceled += StopJumping;
    }

    private void Freeze(InputAction.CallbackContext obj)
    {
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
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
        lastMove = callbackContext.ReadValue<float>();
        moveVector = callbackContext.ReadValue<float>();
    }


    private void Update()
    {
        float jumpValue = CustomInput.Player.Jump.ReadValue<float>();
        if (jumpValue == 1 && isJumping)
        {
            if (jumpTime > 0)
            {
                rigidbody2D.velocity = Vector2.up * jumpForce;
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        float oldY = rigidbody2D.velocity.y;
        float x = moveVector * Speed;
        rigidbody2D.velocity = new Vector2(x, oldY);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            rigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);

            isJumping = true;
            jumpTime = jumpStartTime;
            rigidbody2D.velocity = Vector2.up * jumpForce;
        }
    }

    private void StopJumping(InputAction.CallbackContext obj)
    {
        isJumping = false;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }
}