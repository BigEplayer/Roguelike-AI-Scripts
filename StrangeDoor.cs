using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrangeDoor : MonoBehaviour
{
    //add onDisable() to unsub
    [SerializeField] GameObject activator;
    Animator animator;

    AnimatorStateInfo currentStateInfo;

    float currentStateDuration;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        activator.GetComponent<IDoorActivator>().OnActivate += UpdateActivationState;
    }

    private void Update()
    {
        currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        currentStateDuration = currentStateInfo.normalizedTime;
    }

    private void UpdateActivationState(bool isDeactivated)
    {
        if (isDeactivated)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    private void Deactivate()
    {
        animator.SetBool("isDeactivated", true);
    }

    private void Activate()
    {
        animator.SetBool("isDeactivated", false);

        if (currentStateInfo.IsName("Strange_Door_Deactivate") && currentStateDuration < 1)
        {
            animator.Play("Strange_Door_Activate", 0, Mathf.Abs(currentStateDuration - 1));
        }
    }

    private void OnDisable()
    {
        if(activator != null)
        {
            activator.GetComponent<IDoorActivator>().OnActivate -= UpdateActivationState;
        }
    }
}
