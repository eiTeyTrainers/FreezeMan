using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    Collider2D TrampolineCollision;
    private Rigidbody2D _playerRb2;
    private PlayMovement _playerMovement;
    private Vector3 _lastVelocity;
    private Vector2 _bounceVelocity;
    public float bounceFactor = 20f;
    
    void Start()
    {
        TrampolineCollision = GetComponent<Collider2D>();
        TrampolineCollision.enabled = false;
    }

    public void EnableTrampolineCollision()
    {
        TrampolineCollision.enabled = true;
    }

    private void Update()
    {
        if (_playerRb2)
        {
            Debug.Log(_lastVelocity);
            //TODO: somehow this returns zero vectors
            _lastVelocity = new Vector3(_playerMovement.lastMove, _playerRb2.velocity.y, 0f);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerMovement = other.gameObject.GetComponent<PlayMovement>();
            _playerRb2 = other.gameObject.GetComponent<Rigidbody2D>();
            var speed = _lastVelocity.magnitude;
            Debug.Log(speed);

            var direction = Vector3.Reflect(_lastVelocity.normalized, other.contacts[0].normal);
            _bounceVelocity = direction * Mathf.Max(speed, bounceFactor);
            Debug.Log("Contacts" + other.contacts.Length);
            
            if (_bounceVelocity.magnitude < 1)
            {
                //need to get our own normal for this to work, but this doesnt work
                _bounceVelocity = other.contacts[0].normal.normalized * bounceFactor;
            }
            
            _playerRb2.velocity = _bounceVelocity;
        }
    }
}