using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Player;
    public Transform destination;
    private bool TeleportIn = false;
    private bool Teleportout = false;
    private PlayMovement _playerMovement;
    private SpriteRenderer _spriterenderer;
    public float amp;
    // private float alpahColor = 255;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = Player.GetComponent<PlayMovement>();
        transform.Find("E").gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && destination != null)
        {
            transform.Find("E").gameObject.SetActive(true);

            _spriterenderer = other.GetComponent<SpriteRenderer>();
            if (_playerMovement.CustomInput.Player.Teleport.IsPressed())
            {
                StartCoroutine(PortalIn());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.Find("E").gameObject.SetActive(false);
    }

    private void Update()
    {
        if (transform.Find("E").gameObject.activeSelf)
        {
            transform.Find("E").gameObject.transform.position =
                new Vector2(gameObject.transform.position.x, Mathf.Sin(Time.time) * amp);
        }
    }

    void FixedUpdate()
    {
        if (TeleportIn)
        {
            Color currentColor = _spriterenderer.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                currentColor.a -= Time.fixedDeltaTime * 1.5f);

            _spriterenderer.color = newColor;
        }

        if (Teleportout)
        {
            Color currentColor = _spriterenderer.color;
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                currentColor.a += Time.fixedDeltaTime * 1.5f);
            _spriterenderer.color = newColor;
        }
    }


    private IEnumerator PortalIn()
    {
        TeleportIn = true;
        _playerMovement.CustomInput.Disable();

        yield return new WaitForSeconds(0.5f);
        Player.transform.position = destination.transform.position;
        Teleportout = true;
        TeleportIn = false;
        yield return new WaitForSeconds(0.5f);
        _playerMovement.CustomInput.Enable();
        Teleportout = false;
    }
}