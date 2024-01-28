using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FreezeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayMovement>().Freeze(new InputAction.CallbackContext());
            Destroy(other.gameObject);
            MagazineOfShapes magazineOfShapes = GameObject.Find("gMagazineShapes").GetComponent<MagazineOfShapes>();

            if (magazineOfShapes.shapes.Length == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
