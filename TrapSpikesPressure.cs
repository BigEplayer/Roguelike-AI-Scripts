using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrapSpikesPressure : MonoBehaviour
{
    //  Clean up and fix up both colliders for the pressure spikes
    //  Maybe put basic trap script in parent object and configure 
    //  for it to work there instead

    [SerializeField] int[] interactableLayers;
    [SerializeField] float deactivateSpikesTime = 2f;
    private bool isDeactivating;
    
    [SerializeField] Collider2D hurtCollider;
    Coroutine spikeCoroutine;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Array.Exists(interactableLayers, x => x == collision.gameObject.layer))
        {
            //  must put layer in front of name
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Deactive"))
            {
                animator.SetTrigger("activate");
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Array.Exists(interactableLayers, x => x == collision.gameObject.layer))
        {
            //  must put layer in front of name
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Deactive"))
            {
                animator.SetTrigger("activate");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Array.Exists(interactableLayers, x => x == collision.gameObject.layer))
        {
            if (!isDeactivating && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Deactivate"))
            {
                isDeactivating = true;
                spikeCoroutine = StartCoroutine(DeactivateSpikes());
            }
        }
    }

    private IEnumerator DeactivateSpikes()
    {
        yield return new WaitForSeconds(deactivateSpikesTime);
        animator.SetTrigger("deactivate");

        //  the split second between setting isDeactivating false and the animation actually switching
        //  states (despite the trigger being set on the same frame as as isDeactivating being set false)
        //  allows for player to run the coroutine again, causing the instant retraction bug, also shows
        //  why it would be so rare (?).

        yield return 0; //  waiting one extra frame to disable may fix the issue by removing the possible gap
        isDeactivating = false;
    }

    /*
    public void ModifyCanHurt(int boolean)
    {
        if (boolean == 0)
        {
            canHurt = false;
        }
        else if (boolean == 1) 
        {
            canHurt = true;
        }
        else
        {
            Debug.LogError("Invalid index for modification * ");
        }
    }
    */
}
