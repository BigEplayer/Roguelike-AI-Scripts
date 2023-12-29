using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrapBasic : MonoBehaviour
{
    //[SerializeField] LayerMask interactableLayers;
    [SerializeField] int[] interactableLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //  Array.Exists uses System namespace
        if (Array.Exists(interactableLayers, x => x == collision.gameObject.layer))
        {
            var health = collision.gameObject.GetComponent<IHealth>();
            if (health != null)
            {
                health.HandleTrapHit();
            }
        }
    }
}
