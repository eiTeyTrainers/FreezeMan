using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public float jumpTime;
    public bool isJumping;
    private bool isGrounded = true;

    private const float coyoteTime = 0.2f;

    private float coyoteTimeCounter;

    private const float jumpBufferTime = 0.2f;
    private float jumpBufferTimeCounter;

    //freeze 
    private Transform SpawnPosition;
    public bool isFrozen = false;
    public float lastMove = 0;
    private GameObject magazineObject;
    private gameMode globalGameMode;
    private CinemachineVirtualCamera VCam;
    private AudioClip[] FreezeSounds;
    private AudioSource audioSource;
    public UnityEvent onFreeze;
    
    void Awake()
    {
        CustomInput = new InputControls();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SpawnPosition = GameObject.Find("SpawnPoint").transform;
        globalGameMode = FindObjectOfType<gameMode>();
        GameObject VCamComponent = GameObject.Find("VCam");

        if (VCamComponent)
        {
            VCam = VCamComponent.GetComponent<CinemachineVirtualCamera>();
            VCam.Follow = gameObject.transform;
        }
        
        FreezeSounds = Resources.LoadAll<AudioClip>("Sounds");
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        magazineObject = GameObject.Find("gMagazineShapes");
    }

    private void OnEnable()
    {
        if (enabled && gameObject)
        {
            CustomInput.Enable();
            CustomInput.Player.Move.performed += Move;
            CustomInput.Player.Move.canceled += StopMove;
            CustomInput.Player.Freeze.performed += Freeze;
            CustomInput.Player.Jump.performed += JumpButtonPressed;
            CustomInput.Player.Jump.canceled += StopJumping;
            CustomInput.Player.Restart.canceled += RestartButtonPressed;
        }
    }

    private void OnDisable()
    {
        CustomInput.Disable();
        CustomInput.Player.Move.performed -= Move;
        CustomInput.Player.Move.canceled -= StopMove;
        CustomInput.Player.Freeze.performed -= Freeze;
        CustomInput.Player.Jump.performed -= JumpButtonPressed;
        CustomInput.Player.Jump.canceled -= StopJumping;
        CustomInput.Player.Restart.canceled -= RestartButtonPressed;
    }
    
    private void Freeze(InputAction.CallbackContext obj)
    {
        SpriteSwitcher spriteSwitcher = GetComponent<SpriteSwitcher>();
        if (spriteSwitcher != null)
        {
            spriteSwitcher.isFrozen = true;
        }

        onFreeze.Invoke();
        gameObject.tag = "FrozenPlayer";
        int soundIndex = Random.Range(0, FreezeSounds.Length - 1);
        Debug.Log(FreezeSounds[soundIndex].name);
        audioSource.PlayOneShot(FreezeSounds[soundIndex]);
        
        isFrozen = true;
        globalGameMode.FreezeCounter++;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        int newLayer = LayerMask.NameToLayer("Ground");
        gameObject.layer = newLayer;
        GameObject colliderObject = gameObject.transform.Find("Collider")?.gameObject;
        if (colliderObject != null)
        {
            colliderObject.layer = newLayer;
        }

        CustomInput.Disable();
        MagazineOfShapes magazineScript = magazineObject.GetComponent<MagazineOfShapes>();
        Instantiate(magazineScript.resetUI(), SpawnPosition.position, Quaternion.identity);
    
    }

    private void StopMove(InputAction.CallbackContext obj)
    {
        moveVector = 0;
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
        if (IsCollisionGround(other))
        {
            isGrounded = true;
        }
    }

    private void RestartButtonPressed(InputAction.CallbackContext obj)
    {
        RestartLevel();
    }
    
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}