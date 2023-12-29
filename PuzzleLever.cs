using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleLever : MonoBehaviour, IDoorActivator
{
    public event Activate OnActivate;
    private bool isActive = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Switch()
    {
        isActive = !isActive;

        animator.SetBool("isOn", isActive);
        if(OnActivate != null)
        {
            OnActivate(isActive);
        }
    }
}
