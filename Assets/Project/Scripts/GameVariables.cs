using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    public static GameVariables Instance { get; private set; }

    public static int Coins;
    public static int Health = 3;
    public static int Chapter = 1;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int GetHealth()
    {
        return Health;
    }

    public void SetHealth(int health)
    {
        Health = health;
    }

    public void AdvanceChapter()
    {
        Chapter++;
    }

    public int GetChapter()
    {
        return Chapter;
    }

    public void CoinCollected(int coin)
    {
        Coins += coin;
    }

    public int GetCoins()
    {
        return Coins;
    }
}
