using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public Animator animator;
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void FadeToNextLevel()
    {
        animator.SetBool(FadeOut,true);
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetBool(FadeOut,false);
    }
}
