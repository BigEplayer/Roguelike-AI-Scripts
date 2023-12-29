using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    //  add max hp so hearts can be added without fully healing?
    //  max hp also may be needed to totally remove (disable and destroy) hearts after game has started

    [SerializeField] List<Image> hearts;

    [SerializeField] Image heartPrefab;
    PlayerHealth playerHealth;
    int numOfHearts;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        numOfHearts = (int)playerHealth.health;
    }

    void Update()
    {
        if(playerHealth.health > numOfHearts)
        {
            numOfHearts++;
        }

        if (numOfHearts > hearts.Count)
        {
            //  can also be done with GetComponent<RectTransform>().localPosition, or even 
            //  GetComponent<RectTransform>().anchoredPosition
            var targetPos = hearts[hearts.Count - 1].transform.localPosition + new Vector3(110f, 0f);
            GameObject heart = Instantiate(heartPrefab.gameObject, targetPos, Quaternion.identity);
            heart.transform.SetParent(gameObject.transform, false);
            var heartImage = heart.GetComponent<Image>();
            hearts.Add(heartImage);
        }

        //  for loop and all its iterations are called per frame
        //  Does not stop when iterations are done, atleast in update.
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < playerHealth.health)
            {
                //hearts[i].sprite = fullHeart;

                var animator = hearts[i].GetComponent<Animator>();
                animator.SetBool("isHeartFull", true);
            }
            else
            {
                //hearts[i].sprite = emptyHeart;

                var animator = hearts[i].GetComponent<Animator>();
                animator.SetBool("isHeartFull", false);
            }

            if (i >= numOfHearts)
            {
                Destroy(hearts[i].gameObject);
                hearts.Remove(hearts[i].GetComponent<Image>());
            }
        }
    }
}
