using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    private int currentSpriteIndex = 0;
    private gameMode globalGameMode;
    public bool isFrozen;
    public Sprite FrozenSprite;
    public Sprite WonSprite;
    private void Start()
    {
        // Find the gameMode in the scene
        globalGameMode = FindObjectOfType<gameMode>();
        
        currentSpriteIndex = (int)(Mathf.Floor(globalGameMode._freezeCounter) / 5) ;
        SwitchSprite();
        
        if (globalGameMode == null)
        {
            Debug.LogError("gameMode not found in the scene!");
        }
        else
        {
            // Subscribe to the event
            globalGameMode.FreezeCounterChanged += OnFreezeCounterChanged;
        }

        UpdateSprite();
    }

    private void OnDestroy()
    {
        globalGameMode.FreezeCounterChanged -= OnFreezeCounterChanged;

    }

    private void OnFreezeCounterChanged(float newFreezeCounter)
    {
        // Check if the component is still valid
          SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            // The component is destroyed, so return and do not proceed
            return;
        }

        // Check if the new freeze counter is a multiple of 5
   
            SwitchSprite();
   

        if (isFrozen)
        {
            spriteRenderer.sprite = FrozenSprite;
        }
    }

    private void SwitchSprite()
    {
        currentSpriteIndex = (int)(Mathf.Floor(globalGameMode._freezeCounter) / 5) ;
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        // Assuming you have a SpriteRenderer component attached to the same GameObject
        if (sprites.Count > 0)
        {
            GetComponent<SpriteRenderer>().sprite = sprites[currentSpriteIndex];
        }
        else
        {
            Debug.LogError("Sprites list is empty!");
        }
    }
}