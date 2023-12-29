using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDart : MonoBehaviour
{
    [SerializeField] LayerMask hitableLayerMasks;
    [SerializeField] int[] InteractableLayers; //

    [SerializeField] Dart dart;
    [SerializeField] AudioClip shootSound;
    [SerializeField] bool automaticShooting = false;
    [SerializeField] bool hasAmmo = true;
    [SerializeField] int dartAmmo = 1;
    [SerializeField] float dartSpeed;
    [SerializeField] float dartVarianceY = 0.1f;
    private float dartsLeft;

    [SerializeField] float dartRate = 0.5f;
    private float lastDartTime = Mathf.NegativeInfinity;

    private bool isFliped = false;

    private Animator animator;
    private Collider2D myCollider2D;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myCollider2D = GetComponent<Collider2D>();
        if(transform.localScale.x < 0)
        {
            isFliped = true;
        }

        dartsLeft = dartAmmo;
    }

    void Update()
    {
        RaycastHit2D raycastHit;

        if (!isFliped)
        {
            raycastHit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, hitableLayerMasks);
        }
        else
        {
            raycastHit = Physics2D.Raycast(transform.position, -transform.right, Mathf.Infinity, hitableLayerMasks);
        }

        //   must check if there has even been a hit because if there hasnt been one trying to get the gameobject will cause error
        if (!automaticShooting && raycastHit && raycastHit.transform.gameObject.layer == 10)
        {
            Trigger();
        }
        else if (automaticShooting)
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Trap_Dart_Shoot")
                && Time.time >= lastDartTime + 1 / dartRate)
        {
            if (hasAmmo && dartsLeft > 0)
            {
                AudioSource.PlayClipAtPoint(shootSound, GameComponents.Instance.audioListener.transform.position);

                animator.SetTrigger("shoot");
                lastDartTime = Time.time;

                dartsLeft--;
            }
            else if (!hasAmmo)
            {
                AudioSource.PlayClipAtPoint(shootSound, GameComponents.Instance.audioListener.transform.position);

                animator.SetTrigger("shoot");
                lastDartTime = Time.time;
            }
        }
    }

    public void Shoot()
    {
        
        Dart dart = Instantiate(this.dart, transform.position, transform.rotation);

        if (isFliped)
        {
            dart.transform.localScale = new Vector2(-dart.transform.localScale.x, dart.transform.localScale.y);
            dart.Initialize(dartSpeed, -transform.right);
        }
        else
        {
            dart.Initialize(dartSpeed, transform.right);
        }

        /*
        var startingVector = (transform.rotation * Vector2.right).normalized;
        var rotatedVector = Quaternion.Euler(0, 0, 90) * startingVector;
        var correctRotatedVector = rotatedVector * Random.Range(-dartVarianceY, dartVarianceY);
        dart.transform.position += correctRotatedVector;
        */

        var randomVarianceY = Random.Range(-dartVarianceY, dartVarianceY) * dart.transform.up.normalized;
        dart.transform.position += randomVarianceY;
        
        foreach(Collider2D dartCollider in dart.GetComponents<Collider2D>())
        {
            Physics2D.IgnoreCollision(myCollider2D, dartCollider);
        }
    }
}
