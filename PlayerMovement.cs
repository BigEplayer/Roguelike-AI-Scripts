using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //  fix up dashing
    //  sliding happening again

    [Header("Run Config")]
    [SerializeField] float movementSpeed = 5f;

    [Header("Jump Config")]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] int extraJumpsValue;
    int extraJumps;

    [Header("Dash Config")]
    [SerializeField] bool allowDash = false;
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] float doubleTapWait = 0.3f;
    float doubleTapTime;
    KeyCode lastKeyCode;

    [Header("Misc Config")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject beginDust;
    [SerializeField] private GameObject afterDust;
    [SerializeField] public Transform feetTransform;
    [SerializeField] private float feetRange = 0.25f;
    [SerializeField] private float pushRange = 0.3f;

    //  states
    public bool canMove;
    public bool isFacingRight;
    public bool isDashing;
    public bool isPushing;
    public bool isRunning;
    public bool isAttacking;
    public bool inAir; //
    public bool isGrounded;

    PlayerHealth playerHealth;
    Rigidbody2D myRigidbody2D;
    Animator animator;

    void Start()
    {
        extraJumps = extraJumpsValue;
        playerHealth = GetComponent<PlayerHealth>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAnimationStates();
        Movement();

        HandleJumpVelocity(); //

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

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Air"))
        {
            inAir = true;
        }
        else
        {
            inAir = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Unmovable"))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    private void Movement()
    {
        if (canMove && !playerHealth.isDead)
        {
            if (!isDashing)
            {
                Run();
                Push();
                Turn();
                Jump();

                if (allowDash)
                {
                    Dash();
                }
            }
            
        }
        else
        {
            myRigidbody2D.velocity = new Vector2(0f, 0f);
        }
    }
    
    private void Dash()
    {
        //  SUDO:
        //  put the high velocity on for a set amount of time
        //  Prevent other velocity.x changing things from working (such as running) during this time

        //  left  
        if (Input.GetKeyDown(KeyCode.A) && !isDashing)
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
            {
                StartCoroutine(Dashing(-1));
            }
            else
            {
                doubleTapTime = Time.time + doubleTapWait;
            }

            lastKeyCode = KeyCode.A;
        }

        //  right
        if (Input.GetKeyDown(KeyCode.D) && !isDashing)
        {
            if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
            {
                StartCoroutine(Dashing(1));
            }
            else
            {
                doubleTapTime = Time.time + doubleTapWait;
            }

            lastKeyCode = KeyCode.D;
        }
    }

    IEnumerator Dashing (float direction)
    {
        isDashing = true;

        animator.SetBool("isDashing", true);
        myRigidbody2D.velocity = new Vector2(0f, myRigidbody2D.velocity.y / 2);
        myRigidbody2D.AddForce(new Vector2(dashSpeed * direction, 0f), ForceMode2D.Impulse);
        transform.localScale = new Vector3(direction, 1f);
        yield return new WaitForSeconds(dashTime);
        animator.SetBool("isDashing", false);

        isDashing = false;
    }

    private void Run()
    {
        if (!isAttacking)
        {
            myRigidbody2D.velocity = new Vector3
                       (Input.GetAxis("Horizontal") * movementSpeed, myRigidbody2D.velocity.y);

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon)
            {
                animator.SetBool("isRunning", true);
                isRunning = true;
            }
            else
            {
                animator.SetBool("isRunning", false);
                isRunning = false;
            }
        }
        else if (isAttacking)
        {
            myRigidbody2D.velocity = new Vector2(0f, myRigidbody2D.velocity.y);
        }
    }

    private void Turn()
    {
        if (!isAttacking)
        {
            if (Mathf.Abs(Input.GetAxis("Horizontal")) != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(Input.GetAxis("Horizontal")), 1f);
                if (Mathf.Sign(Input.GetAxis("Horizontal")) > 0)
                {
                    isFacingRight = true;
                }
                else if (Mathf.Sign(Input.GetAxis("Horizontal")) < 0)
                {
                    isFacingRight = false;
                }
            }
            
        }
    }

    private void Push()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Epsilon)
        {
            if (isGrounded && isFacingRight && Physics2D.Raycast(transform.position, Vector2.right, pushRange, groundLayer))
            {
                Debug.DrawRay(transform.position, Vector2.right, Color.green);
                animator.SetBool("isPushing", true);
                isPushing = true;
            }
            else if (isGrounded && !isFacingRight && Physics2D.Raycast(transform.position, Vector2.left, pushRange, groundLayer))
            {
                Debug.DrawRay(transform.position, Vector2.left, Color.green);
                animator.SetBool("isPushing", true);
                isPushing = true;
            }
            else
            {
                animator.SetBool("isPushing", false);
                isPushing = false;
            }
        }
        else
        {
            animator.SetBool("isPushing", false);
            isPushing = false;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
            animator.SetBool("isDoubleJumping", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded == false && extraJumps > 0)
        {
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpForce);
            animator.SetTrigger("jumpStart");
            animator.SetBool("isDoubleJumping", true);
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpForce);
            Instantiate(beginDust, transform.position, Quaternion.identity);
            animator.SetTrigger("jumpStart");
        }

        if (Input.GetButtonUp("Jump") && myRigidbody2D.velocity.y > 0)
        {
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, myRigidbody2D.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void HandleJumpVelocity()
    {
        //  removed isJumping as it was a little messy and repetitive. Did the same in the animator, where I removed
        //  the transition from jumping down and jumping up to player land. Instead, I based it off player jumping down.
        //  could cause issues where hero does not play landing animation as there is no transition to it from player jumping up

        if (myRigidbody2D.velocity.y > 0.1f)
        {
            animator.SetBool("isJumpingUp", true);
            animator.SetBool("isJumpingDown", false);
        }
        else if (myRigidbody2D.velocity.y < -0.1f)
        {
            animator.SetBool("isJumpingDown", true);
            animator.SetBool("isJumpingUp", false);
        }
        else if (isGrounded)
        {
            animator.SetBool("isJumpingDown", false);
            animator.SetBool("isJumpingUp", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (feetTransform == null) { return; }

        Gizmos.DrawWireSphere(feetTransform.position, feetRange);
    }
}
