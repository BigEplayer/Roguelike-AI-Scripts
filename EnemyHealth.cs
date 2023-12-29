using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    //  find a more sophisticated and safe way of doing death animation
    //  !! other states using any state may cancel death animation, be wary.

    [Header("Health Basics Config")]
    [SerializeField] float health = 10f;
    [SerializeField] GameObject hitParticles;
    [SerializeField] AudioClip hurtSound;
    bool isDead = false;
    
    [Header("Knockback Config")]
    public bool isKnockedback = false;
    [SerializeField] Vector2 knockbackForce;
    [SerializeField] float knockbackDuration;
    float knockbackStart;

    bool playerFacingRight;

    Rigidbody2D myRigidBody2D;
    Animator animator;

    private void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isKnockedback)
        {
            KnockbackCheck();
        }
    }

    public void TakeHit(float damage)
    {
        if (!isDead)
        {
            AudioSource.PlayClipAtPoint(hurtSound, GameComponents.Instance.audioListener.transform.position);

            animator.SetTrigger("hurt");
            Knockback();

            if (hitParticles)
            {
                //  starts with random rotation between 0 and 360.
                Instantiate(hitParticles, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)));
            }

            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void HandleTrapHit()
    {
        if (!isDead)
        {
            AudioSource.PlayClipAtPoint(hurtSound, GameComponents.Instance.audioListener.transform.position);

            health = 0;
            animator.SetTrigger("hurt");
            Die();
        }
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        animator.SetBool("isDead", true);
        isDead = true;
    }

    private void Knockback()
    {
        isKnockedback = true;
        knockbackStart = Time.time;

        Debug.Log(playerFacingRight);
        if (playerFacingRight)
        {
            myRigidBody2D.velocity = new Vector2(knockbackForce.x, knockbackForce.y);
        }
        else
        {
            myRigidBody2D.velocity = new Vector2(-knockbackForce.x, knockbackForce.y);
        }
        
    }

    private void KnockbackCheck()
    {
        if(Time.time >= knockbackStart + knockbackDuration && isKnockedback)
        {
            isKnockedback = false;
        }
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    public void AssignPlayerDirection(bool isFacingRight)
    {
        if (isFacingRight)
        {
            playerFacingRight = true;
        }
        else
        {
            playerFacingRight = false;
        }
    }
}
