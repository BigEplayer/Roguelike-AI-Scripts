using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //  Adjust the attack so it lasts for a bit instead of just hitting during the first frame.

    [Header("Slash Config")]
    [SerializeField] float damage = 5f;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius = 0.25f;
    [SerializeField] LayerMask interactableLayers;

    [SerializeField] float attackRate = 1f;
    float lastAttackTime = Mathf.NegativeInfinity;

    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    Animator animator;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Attack();
    }
    
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastAttackTime)
        {
            if (!playerMovement.isAttacking && !playerMovement.inAir && !playerMovement.isPushing
                && !playerHealth.isDead && playerMovement.canMove && !playerHealth.isHurt)
            {
                animator.SetTrigger("attack");
                lastAttackTime = Time.time + 1 / attackRate;
            }
        }
    }

    public void AttemptHit()
    {
        Collider2D[] hitInteractables = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, interactableLayers);

        foreach (Collider2D interactable in hitInteractables)
        {
            if(interactable.GetComponent<EnemyHealth>())
            {
                //  looks for every method in the gameobject named the string and then passes in a value.
                interactable.SendMessage("AssignPlayerDirection", playerMovement.isFacingRight);
                interactable.SendMessage("TakeHit", damage);
            }
            else if(interactable.GetComponent<PuzzleLever>())
            {
                interactable.GetComponent<PuzzleLever>().Switch();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null) { return; }

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
