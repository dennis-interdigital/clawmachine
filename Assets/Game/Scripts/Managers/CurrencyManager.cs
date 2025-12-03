using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager
{
    GameManager gameManager;
    UserData userData;

    public void Init(GameManager inGameManager)
    {
        gameManager = inGameManager;
        userData = gameManager.userData;
    }

    public bool IsCoinSufficient(int cost)
    {
        int coin = userData.coin;
        bool result = coin >= cost;

        return result;
    }

    public void SpendCoin(int cost)
    {
        userData.coin -= cost;
    }

    public void AddCoin(int amount)
    {
        userData.coin += amount;
    }
}
