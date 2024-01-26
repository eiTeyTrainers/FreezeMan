using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangerTrigger : MonoBehaviour
{
    private NextScene NextScene;
    // Start is called before the first frame update
    private void Start()
    {
        NextScene = GameObject.Find("LevelChanger").GetComponent<NextScene>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NextScene.FadeToNextLevel();
        }
    }
}
