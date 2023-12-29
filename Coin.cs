using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Coin : MonoBehaviour
{
    //[SerializeField] LayerMask interactableLayerMask;
    [SerializeField] int[] interactableLayers;

    [SerializeField] int coinValue = 1;
    [SerializeField] AudioClip coinSound;
    CoinUI coinUI;
    Animator animator;

    private void Start()
    {
        coinUI = FindObjectOfType<CoinUI>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //  !! did this a different way using layermask instead of ints
        if (Array.Exists(interactableLayers, x => x == collision.gameObject.layer))
        {
            AudioSource.PlayClipAtPoint(coinSound, GameComponents.Instance.audioListener.transform.position);

            animator.SetTrigger("coinCollect");
            coinUI.AddCoins(coinValue);
        }
    }

    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
