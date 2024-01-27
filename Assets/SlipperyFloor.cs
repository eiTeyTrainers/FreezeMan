using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlipperyFloor : MonoBehaviour
{
    // Start is called before the first frame update
    private float slippyTime = 0f;
    public float slippyTimeMax = 3f;
    public Vector2 slipperyVector;
    public float slippyforce = 2;
    private Collider2D parentRigidbody;
    public PhysicsMaterial2D OnTrigge_material;


    void Start()
    {
        if (gameObject.transform.parent != null)
        {
            parentRigidbody = gameObject.transform.parent.GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogWarning("SlipperyFloor: No parent with Collider2D found.");
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (slippyTime > 0)
        {
            slippyTime -= Time.fixedDeltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayMovement Player = other.gameObject.GetComponent<PlayMovement>();
        if (Player)
        {
            parentRigidbody.sharedMaterial = OnTrigge_material;

            if (Player.moveVector != 0)
            {
                slippyTime = slippyTimeMax;
            }

            slipperyVector = new Vector2(slippyTime * slippyforce * Player.lastMove, 0);
            Player.GetComponent<Rigidbody2D>().AddForce(slipperyVector, ForceMode2D.Force);
        }
    }
}