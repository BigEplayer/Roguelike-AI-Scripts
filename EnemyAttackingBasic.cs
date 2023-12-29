using UnityEngine;

public class EnemyAttackingBasic : MonoBehaviour
{
    //  TODO:
    //  Maybe make coroutine to wait inbetween attacks
    //  Add animation event to damage the player

    [Header("Attack Config")]
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackDamage = 1f;
    [SerializeField] float attackRadius;

    Animator animator; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        HasHitPlayer();
    }

    private Collider2D HasHitPlayer()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayerMask);

        //  bool instead of a trigger like player
        if (hitPlayer)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        return hitPlayer;
    }

    public void DealHit()
    {
        var player = HasHitPlayer();
        if (player)
        {
            player.GetComponent<PlayerHealth>().TakeHit(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
