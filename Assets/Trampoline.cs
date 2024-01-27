using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    private Rigidbody2D _playerRb2;
    private PlayMovement _playerMovement;
    private Vector3 _lastVelocity;

    void Start()
    {
    }


    private void Update()
    {
        if (_playerRb2)
        {
            Debug.Log(_lastVelocity);
            _lastVelocity = _playerRb2.velocity;
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
            _playerRb2.velocity = direction * Mathf.Max(speed, 20f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        throw new NotImplementedException();
    }
}