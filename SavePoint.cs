using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavePoint : MonoBehaviour
{
    [SerializeField] LayerMask interactableLayers;

    [SerializeField] private SpriteRenderer effectObject;
    [SerializeField] private float exitTime = 2f;

    [SerializeField] private float effectSpeed = 2f;
    [SerializeField] private AudioClip saveSound;

    public bool effectIsTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //  !! get this better !!
        //  single "&" gets the things in common with the both the bitmasks. Ex:
        // in 0000 1100
        // in 0010 0100
        // out 0000 0100

        if (!effectIsTriggered && (interactableLayers.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            AudioSource.PlayClipAtPoint(saveSound, GameComponents.Instance.audioListener.transform.position);

            GameComponents.Instance.levelLoader.LoadNextScene(exitTime);
            GetComponent<Animator>().SetTrigger("activate");
            StartCoroutine(TriggerEffect());

            effectIsTriggered = true;
        }
    }
    
    private IEnumerator TriggerEffect()
    {
        SpriteRenderer effect = Instantiate(effectObject, transform.position, transform.rotation);

        while (true)
        {
            effect.transform.localScale += new Vector3(effectSpeed * Time.deltaTime, effectSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
