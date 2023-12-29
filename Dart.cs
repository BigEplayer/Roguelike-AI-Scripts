using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    //  fix issue with pivot when dart hits the tilemap (issue with composite collider maybe).
    //  fix y offset on anchor

    [SerializeField] float persistanceTime;
    [SerializeField] float opacityDecrease;

    private float dartSpeed;
    private Vector2 dartDirection;

    private Animator animator;
    private Rigidbody2D dartRigidbody2D;
    private Collider2D spearheadCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        dartRigidbody2D = GetComponent<Rigidbody2D>();
        spearheadCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //  able to work in start because Initialize is being run before it.
        dartRigidbody2D.velocity = dartDirection * dartSpeed;
    }

    public void Initialize(float dartSpeed, Vector2 dartDirection)
    {
        this.dartSpeed = dartSpeed;
        this.dartDirection = dartDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>()) { return; }

        dartRigidbody2D.velocity = Vector2.zero;

        FixedJoint2D joint = gameObject.AddComponent<FixedJoint2D>();
        //joint.anchor = (Vector2)spearheadCollider.bounds.center + new Vector2(spearheadCollider.bounds.extents.x, 0);

        //  ClosetPoint gets the closet point to a position within the bounds of the collider
        joint.anchor = transform.InverseTransformPoint(collision.ClosestPoint(transform.position));

        //  connectedBody lets the joint follow an object (the one connected)
        joint.connectedBody = collision.attachedRigidbody;
        joint.enableCollision = false;

        //dartRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        //transform.SetParent(collision.transform);

        foreach(Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        animator.speed = 0;

        StartCoroutine(DartEffect());
    }
    
    IEnumerator DartEffect()
    {
        Color matColor = spriteRenderer.material.color;

        yield return new WaitForSeconds(persistanceTime);

        while (true)
        {
            if(!(matColor.a <= 0))
            {
                matColor.a -= opacityDecrease * Time.deltaTime;
                spriteRenderer.material.color = matColor;
            }
            else
            {
                Destroy(gameObject);
            }

            yield return 0;
        }
    }
}
