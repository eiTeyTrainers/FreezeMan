using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlipperyFloor : MonoBehaviour
{
    // Start is called before the first frame update
    public float slippyTime = 0f;
    public Vector2 amongus;
    private float slippyforce = 20;
    private Collider2D _Collider2Dbody;
    public PhysicsMaterial2D OnTrigge_material;


    void Start()
    {
        _Collider2Dbody = gameObject.transform.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (slippyTime > 0)
        {
            slippyTime -= Time.fixedDeltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        PlayMovement Player = other.gameObject.GetComponent<PlayMovement>();
        if (Player)
        {
            _Collider2Dbody.sharedMaterial = OnTrigge_material;

            if (Player.moveVector != 0)
            {
                slippyTime = 3f;
            }

            amongus = new Vector2(slippyTime * slippyforce * Player.lastMove, 0);
            Debug.Log(Player.moveVector);

            Player.GetComponent<Rigidbody2D>().AddForce(amongus, ForceMode2D.Force);
        }
    }
}