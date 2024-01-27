using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnTrigger : MonoBehaviour
{
    public PhysicsMaterial2D OnTrigge_material;
    public PhysicsMaterial2D OnTriggerExit_material;
    private PlayMovement parent;
    private Rigidbody2D parentRigidbody;


    void Start()
    {
        parent = gameObject.transform.parent.GetComponent<PlayMovement>();
        parentRigidbody = gameObject.transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (parent != null && parent.isFrozen && parentRigidbody)
        {
            parentRigidbody.sharedMaterial = OnTrigge_material;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (parent != null && parent.isFrozen && parentRigidbody)
        {
            parentRigidbody.sharedMaterial = OnTriggerExit_material;
        }
    }
}