using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameVariables : MonoBehaviour
{
    public static GameVariables Instance { get; private set; }

    public static int Coins = 999;
    public static int Health = 3;
    public static int HealthContainers = 3;
    public static int Chapter = 1;
    public static FlappyStage LevelPicked;
    public static bool PressedStart = true;
    public static string SpecialLevel;
    public static FlappyItem[] slots = { null, null, null };
    public static List<Shop.ShopList> shops = new List<Shop.ShopList>();
    public static List<FlappyItem> Inventory = new List<FlappyItem>();
    public static Shop.ShopList ShopToLoad;

    public static FlappyWings Wings;
    public static FlappyBody Body;
    public static FlappyEyes Eyes;
    public static FlappyHat Hat;
    public static FlappyBeak Beak;

    public ItemList ItemList;
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

    public void GenerateAShop()
    {
        Shop.ShopList newShop = new Shop.ShopList();
        newShop.items = new List<Shop.ItemForSale>();
        for (int i = 0; i < Random.Range(4, 6); i++)
        {
            Shop.ItemForSale newItem = new Shop.ItemForSale();
            newItem.product = ItemList.items[Random.Range(0, ItemList.items.Length)];
            newItem.price = Random.Range(3, 7);
            newShop.items.Add(newItem);
        }
        shops.Add(newShop);
    }

    public List<Shop.ShopList> GetShops()
    {
        return shops;
    }

    public int GetHealth()
    {
        return Health;
    }

    public void ReplaceShop(Shop.ShopList newShop)
    {
        GetShopToLoad().items = newShop.items;
    }

    public void SetShopToLoad(Shop.ShopList load)
    {
        ShopToLoad = load;
    }

    public Shop.ShopList GetShopToLoad()
    {
        return ShopToLoad;
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

    public string GetSpecialLevel()
    {
        return SpecialLevel;
    }

    public void SetSpecialLevel(string special)
    {
        SpecialLevel = special;
    }

    public bool ConfirmItemExists(string term)
    {
        foreach (FlappyItem item in Inventory) { 
            if (item.ItemName == term) return true;
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

    public void ActiveItemUsed(int used)
    {
        slots[used] = null;
    }

    public void AddItemToSlot(int slot, FlappyItem item)
    {
        slots[slot] = item;
    }

    public FlappyItem[] GetActiveSlots()
    {
        return slots;
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

    public FlappyBeak GetBeak()
    {
        return Beak;
    }

    public void SetBeak(FlappyBeak beak)
    {
        Beak = beak;
    }

    public FlappyHat GetHat()
    {
        return Hat;
    }

    public void SetHat(FlappyHat hat)
    {
        Hat = hat;
    }

    public FlappyEyes GetEyes()
    {
        return Eyes;
    }

    public void SetEyes(FlappyEyes eyes)
    {
        Eyes = eyes;
    }

    public FlappyBody GetBody()
    {
        return Body;
    }

    public void SetBody(FlappyBody body)
    {
        Body = body;
    }

    public FlappyWings GetWings()
    {
        return Wings;
    }

    public void SetWings(FlappyWings wings)
    {
        Wings = wings;
    }
}
