using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    public float health;

    [SerializeField] private float deathTime;
    public bool isHurt = false;
    public bool isDead = false;

    [SerializeField] private AudioClip hurtSound;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeHit(float damage)
    {
        var savePoint = FindObjectOfType<SavePoint>();
        if (!isDead && savePoint ? !savePoint.effectIsTriggered : true)
        {
            AudioSource.PlayClipAtPoint(hurtSound, GameComponents.Instance.audioListener.transform.position);

            animator.SetBool("isHurt", true);
            isHurt = true;

            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void HandleTrapHit()
    {
        var savePoint = FindObjectOfType<SavePoint>();
        if (!isDead && savePoint ? !savePoint.effectIsTriggered : true)
        {
            AudioSource.PlayClipAtPoint(hurtSound, GameComponents.Instance.audioListener.transform.position);

            health = 0;
            animator.SetBool("isHurt", true);
            isHurt = true;
            Die();
        }
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        animator.SetBool("isDead", true);
        isDead = true;

        GameComponents.Instance.levelLoader.ReloadScene(deathTime);
    }

    public void hurtFinished()
    {
        animator.SetBool("isHurt", false);
        isHurt = false;
    }
}
