using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMovement : MonoBehaviour
{
    //  Add is jumping bool

    //  Allow for enemy to jump over gaps.
    //  Allow it to keep moving when attacking when jumping over said gaps so it doesnt fall.

    //  !!!! prevent falling on y axis when unMovable (do same for Hero)

    [SerializeField] LayerMask groundLayer;

    [Header("Movement Config")]
    [SerializeField] bool isMoveRight;
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float jumpRange;

    [Header("Pathing Config")]
    [SerializeField] List<Transform> movePoints;
    [SerializeField] float waitBetweenPoints;
    [SerializeField] float offsetToPreventSpasm = 0.1f;
    [SerializeField] int pointIndex;

    [Header("Misc Config")]
    [SerializeField] Transform feetTransform;
    [SerializeField] float feetRange = 0.25f;

    bool isGoingToAttack;

    public bool unMovable;
    public bool isSwitchingPoints;
    public bool isAttacking;
    public bool isHurt;
    public bool isGrounded;

    EnemyHealth enemyHealth; //
    Animator animator;
    Rigidbody2D myRigidbody2D;

    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>(); //  
        animator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();

        pointIndex = Mathf.Clamp(pointIndex, 0, movePoints.Count);
        
        foreach(Transform point in movePoints)
        {
            point.parent = null;
        }

        if (!isMoveRight)
        {
            movePoints.Reverse();
        }
    }

    private void Update()
    {
        UpdateAnimationStates();
        if (!enemyHealth.isKnockedback)
        {
            Pathing();
        }

        isGrounded = Physics2D.OverlapCircle(feetTransform.position, feetRange, groundLayer);
    }

    private void UpdateAnimationStates()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
        {
            isHurt = true;
        }
        else
        {
            isHurt = false;
        }

        if (animator.GetBool("isAttacking"))
        {
            isGoingToAttack = true;
        }
        else
        {
            isGoingToAttack = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Unmovable"))
        {
            unMovable = true;
        }
        else
        {
            unMovable = false;
        }
    }

    private void LateUpdate()
    {
        //  !isAttacking must be paid attention. May cause issues if attacking when JUMPING ATTACKING is added.

        if (Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon 
            && !isHurt && !unMovable && !isAttacking && !enemyHealth.isKnockedback)
        {
            if (!isGoingToAttack)
            {
                transform.localScale = new Vector3(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
            }
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void Pathing()
    { 
        if (pointIndex <= movePoints.Count - 1)
        {

            if (!isAttacking && !isHurt && !unMovable)
            {
                JumpIfNeeded();

                if (transform.position.x < movePoints[pointIndex].position.x - offsetToPreventSpasm)
                {
                    myRigidbody2D.velocity = new Vector2(movementSpeed, myRigidbody2D.velocity.y);
                }
                else if (transform.position.x > movePoints[pointIndex].position.x + offsetToPreventSpasm)
                {
                    myRigidbody2D.velocity = new Vector2(-movementSpeed, myRigidbody2D.velocity.y);
                }
                else
                {
                    myRigidbody2D.velocity = new Vector2(0f, myRigidbody2D.velocity.y);
                }
            }
            else
            {
                myRigidbody2D.velocity = new Vector2(0f, myRigidbody2D.velocity.y);
            }

            //myRigidbody2D.MovePosition(transform.position);
            //transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed);

            //  can still switch while unMovable, may cause issues.
            var xLowerBound = movePoints[pointIndex].position.x - offsetToPreventSpasm;
            var xHigherBound = movePoints[pointIndex].position.x + offsetToPreventSpasm;
            if (transform.position.x >= xLowerBound && transform.position.x <= xHigherBound)
            {
                if (!isSwitchingPoints)
                {
                    StartCoroutine(SwitchPoints());

                    isSwitchingPoints = true;
                }
            }
        }
        else
        {
            movePoints.Reverse();
            pointIndex = 0;
        }
    }

    IEnumerator SwitchPoints()
    {
        yield return new WaitForSeconds(waitBetweenPoints);
        pointIndex++;

        isSwitchingPoints = false;
    }

    private void JumpIfNeeded()
    {
        RaycastHit2D rayHit;
        Vector3 feetLowerVector = new Vector3(0f, 0.2f);

        if (Mathf.Sign(transform.localScale.x) == 1)
        {
            if (transform.position.x < movePoints[pointIndex].position.x - offsetToPreventSpasm)
            {
                rayHit = Physics2D.Raycast(transform.position - feetLowerVector, transform.TransformDirection(Vector2.right), jumpRange, groundLayer);
                Debug.DrawLine(transform.position - feetLowerVector, (transform.position - feetLowerVector) + new Vector3(jumpRange, 0f), Color.green);

                if (rayHit && isGrounded)
                {
                    myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpHeight);
                }
            }
        }
        else
        {
            if (transform.position.x > movePoints[pointIndex].position.x + offsetToPreventSpasm)
            {
                rayHit = Physics2D.Raycast(transform.position - feetLowerVector, transform.TransformDirection(Vector2.left), jumpRange, groundLayer);
                Debug.DrawLine(transform.position - feetLowerVector, (transform.position - feetLowerVector) + new Vector3(-jumpRange, 0f), Color.green);


                if (rayHit && isGrounded)
                {
                    myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpHeight);
                }
            }
        }

        
    }

    void OnDrawGizmosSelected()
    {
        if (feetTransform == null) { return; }

        Gizmos.DrawWireSphere(feetTransform.position, feetRange);
    }
}
