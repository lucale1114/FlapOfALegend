using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    public static GameVariables Instance { get; private set; }

    public static int Coins;
    public static int Health = 3;
    public static int HealthContainers = 3;
    public static int Chapter = 1;
    public static FlappyStage LevelPicked;
    public static bool PressedStart = true;
    public static string ConsumableSlot1;
    public static string ConsumableSlot2;
    public static string ConsumableSlot3;
    public static List<FlappyItem> Inventory = new List<FlappyItem>();


    public AudioClip ButtonClickSound;

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

    public void AddItemToInventory(FlappyItem item)
    {
        Inventory.Add(item);
    }

    public List<FlappyItem> GetInventory()
    {
        return Inventory;
    }

    public bool ConfirmItemExists(string term)
    {
        foreach (FlappyItem item in Inventory) { 
            if (item.name == term) return true;
        }
        return false;
    }

    public int GetContainers()
    {
        return HealthContainers;
    }

    public void SetContainers(int containers)
    {
        HealthContainers = containers;
    }

    public void AdvanceChapter()
    {
        Chapter++;
    }

    public int GetChapter()
    {
        return Chapter;
    }

    public bool CanInteractSelect()
    {
        return PressedStart;
    }

    public void SetInteractSelect(bool state)
    {
        PressedStart = state;
    }

    public FlappyStage GetLevel()
    {
        return LevelPicked;
    }

    public void ResetVariables()
    {
        Health = 3;
        Chapter = 1;
        LevelPicked = null;
    }

    public void SetLevel(FlappyStage level)
    {
        LevelPicked = level;
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
