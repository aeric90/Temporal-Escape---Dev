using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public static FadeController instance;
    private bool fade_on;
    private Animator animator;

    private void Start()
    {
        if(instance == null) instance = this;
        animator = GetComponent<Animator>();
    }

    public void SetFade(bool value)
    {
        fade_on = value;
        animator.SetBool("Fade Out", fade_on);
    }
}
