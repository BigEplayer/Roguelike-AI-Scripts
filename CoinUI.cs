using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public int currentCoins = 0;
    Text coinText;

    void Start()
    {
        coinText = GetComponentInChildren<Text>();
        coinText.text = (currentCoins + GameComponents.Instance.gameData.totalCoins).ToString();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        coinText.text = (currentCoins + GameComponents.Instance.gameData.totalCoins).ToString();
    }

    public void RemoveCoins(int amount)
    {
        currentCoins -= amount;
        coinText.text = (currentCoins + GameComponents.Instance.gameData.totalCoins).ToString();
    }
}
