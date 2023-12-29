using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrapSpikesFalling : MonoBehaviour
{
    // !! do the raycast based on the bounds of the object, may need to make trigger collider seperate to do so

    //  could use "myRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;" instead of weird constraints

    [SerializeField] LayerMask interactableLayerMask;
    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] private AudioClip dropSound;
    [SerializeField] private Vector2 dropBoxSize;
    [SerializeField] private Vector2 dropBoxDirection = Vector2.down;
    [SerializeField] private float dropBoxDistance = -1f;

    [SerializeField] private SpriteRenderer groundedEffect;
    [SerializeField] private bool isGrounded;
    
    private Rigidbody2D myRigidBody2D;
    private Collider2D myCollider2D;

    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myCollider2D = GetComponent<Collider2D>();

        myRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY
            | RigidbodyConstraints2D.FreezeRotation;

        /*
        var be = 1 << 9 | 1 << 10;
        Debug.Log(be);
        Debug.Log(LayerMask.GetMask("Player", "Enemy"));
        */
    }

    private void FixedUpdate()
    {
        DetectIfShouldDrop();
        DetectIfGrounded();
    }

    private void DetectIfGrounded()
    {
        if (!isGrounded)
        {
            var collisionPadding = 0.05f;

            //  try using center of collider (seems like it may be same as transform but could be rounding).
            //  !! Bounds thing may act wierdly with negative offsets

            var groundedCast = Physics2D.Raycast(myCollider2D.bounds.center, Vector2.down, myCollider2D.bounds.extents.y + collisionPadding, groundLayerMask);
            if (groundedCast)
            {
                myRigidBody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY
                | RigidbodyConstraints2D.FreezeRotation;

                //  maybe raise up where effect is instantiated or change the pivot point as it seems too low
                //  grab collider from foreground and put the effect at the top bound extent of the tiles collider

                Vector3 vectorToSubtract = new Vector3(0f, myCollider2D.bounds.extents.y);
                Instantiate(groundedEffect, myCollider2D.bounds.center - vectorToSubtract, transform.rotation);

                AudioSource.PlayClipAtPoint(dropSound, GameComponents.Instance.audioListener.transform.position);

                isGrounded = true;
            }
        }
    }

    private void DetectIfShouldDrop()
    {
        if (!isGrounded)
        {
            // !! !! replace or serialize the getmask thing here to work with interactable layers !! !!
            //  Don't really need LayerMask.GetMask("Player") at the end since the collisions are being checked for their layers anyways


            //  drop box distance is basically in negative y because the direction of the raycast is pointing down (instead of up).
            RaycastHit2D dropBoxCast = Physics2D.BoxCast
                    (transform.position, dropBoxSize, 0f, dropBoxDirection, dropBoxDistance, interactableLayerMask);

            if (dropBoxCast)
            {
                myRigidBody2D.constraints = ~RigidbodyConstraints2D.FreezePositionY;
            }
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)(dropBoxDirection.normalized * dropBoxDistance), dropBoxSize);
    }
}
