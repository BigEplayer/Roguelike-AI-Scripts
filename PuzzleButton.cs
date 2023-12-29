using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PuzzleButton : MonoBehaviour, IDoorActivator
{
    //  Fix pressure spasm causing the object to enter and exit repeatibly when the
    //  pressure plate is pushed down, repeating the cycle over and over
    //      maybe add a timer inbetween pressing and releasing the pressure plate to prevent spasm
    //      "&& Time.time >= pressTime + pressRate"

    //  maybe fix swing trap colliders as it may prevent death on corners due to shape

    private Collider2D buttonColliders;
    private float collidedObjectAmount;

    public event Activate OnActivate;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        //  First can be used as first thing to meet the conditions
        buttonColliders = GetComponentsInChildren<Collider2D>()
            .First(x => x.transform.parent == transform);
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == buttonColliders)
        {
            if (OnActivate != null && collidedObjectAmount <= 0)
            {
                animator.SetBool("isPushed", true);
                OnActivate(true);
            }

            collidedObjectAmount++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.otherCollider == buttonColliders)
        {
            if (OnActivate != null && collidedObjectAmount <= 1)
            {
                animator.SetBool("isPushed", false);
                OnActivate(false);
            }

            collidedObjectAmount--;
        }
    }
}
