using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public class ItemForSale {
        public FlappyItem product;
        public int price;
        public bool bought;
    }

    public class ShopList
    {
        public List<ItemForSale> items;
    }

    public event System.Action Leaving;

    private Transform shopTab;
    private Vector3 shopTabPos;
    private Vector3 titlePos;
    private Vector3 coinPos;
    private Vector3 coinPos2;
    private Vector3 confirmPos;
    private Transform titleText;
    private Transform coin;
    private GameObject[] shopSlots;
    private bool leaving;
    private ShopList shopList;
    private Transform trans;
    private TextMeshProUGUI coinsAmount;
    private Transform buyConfirm;
    private TextMeshProUGUI confirmTitle;
    private TextMeshProUGUI confirmDescription;
    private TextMeshProUGUI confirmCost;
    private GameObject[] positives;
    private Image confirmIcon;
    private Image fadeSlots;
    private Transform slot1;
    private Transform slot2;
    private Transform slot3;
    private Transform backButton;
    private bool picked;
    [SerializeField]
    private AudioClip shopMusic;
    private ItemForSale focusedItem;
    [SerializeField]
    private AudioClip decreaseCoinSound;
    [SerializeField]
    private AudioClip purchaseSound;
    [SerializeField]
    private AudioClip activeSelected;
    [SerializeField]
    private AudioClip helicopterSound;
    [SerializeField]
    private AudioClip bringUp;

    private void Awake()
    {
        shopTab = transform.Find("ShopTab");
        shopTabPos = shopTab.position;
        titleText = transform.Find("Title");
        coin = transform.Find("Coin");
        buyConfirm = transform.Find("BuyConfirm");
        confirmTitle = buyConfirm.Find("Title").GetComponent<TextMeshProUGUI>();
        confirmDescription = buyConfirm.Find("Desc").GetComponent<TextMeshProUGUI>();
        confirmCost = buyConfirm.Find("Cost").GetComponent<TextMeshProUGUI>();
        confirmIcon = buyConfirm.Find("ImageFrame").GetChild(0).GetComponent<Image>();
        confirmPos = buyConfirm.position;
        positives = GameObject.FindGameObjectsWithTag("Aggregation");
        titlePos = titleText.position;
        fadeSlots = transform.Find("FadeSlots").GetComponent<Image>();
        slot1 = fadeSlots.transform.Find("Slot1");
        slot2 = fadeSlots.transform.Find("Slot2");
        slot3 = fadeSlots.transform.Find("Slot3");
        coinPos = coin.position;
        coinsAmount = GameObject.Find("CoinsAmount").GetComponent<TextMeshProUGUI>();
        coinPos2 = coinsAmount.transform.position;
        shopList = GameVariables.Instance.GetShopToLoad();
        trans = GameObject.Find("TransitionShop").transform;
        shopSlots = GameObject.FindGameObjectsWithTag("Slots");
        backButton = GameObject.Find("ButtonBack").transform;
    }

    void Start()
    {
        StartCoroutine(AudioManager.FadeOutAndPlayNew());
        foreach (GameObject slot in shopSlots)
        {
            slot.SetActive(false);
        }
        AudioManager.LevelMusic = shopMusic;
        AudioManager.PlaySound(helicopterSound, 0.7f, 1, true);
        fadeSlots.gameObject.SetActive(false);
        confirmPos = buyConfirm.position;
        buyConfirm.position -= new Vector3(0, 2500); 
        titleText.position += new Vector3(0, 2000);
        coin.position += new Vector3(0, 2000);
        coinsAmount.transform.position += new Vector3(0, 2000);
        shopTab.position += new Vector3(1000, 0);
        backButton.position += new Vector3(1500, 0);
        Invoke("StartMoving", 2);
        Refresh();
        slot1.GetComponent<Button>().onClick.AddListener(delegate { AddToActive(0); });
        slot2.GetComponent<Button>().onClick.AddListener(delegate { AddToActive(1); });
        slot3.GetComponent<Button>().onClick.AddListener(delegate { AddToActive(2); });
    }

    void RefreshActive(Transform slot, FlappyItem item)
    {
        Image image = slot.GetChild(0).GetComponent<Image>();
        if (item == null)
        {
            image.enabled = false;
            return;
        }
        image.enabled = true;
        image.sprite = item.Icon;

        slot.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.ItemName;
    }

    void Refresh()
    {
        coinsAmount.text = GameVariables.Instance.GetCoins() + "x";
        FlappyItem[] activeSlot = GameVariables.Instance.GetActiveSlots();
        RefreshActive(slot1, activeSlot[0]);
        RefreshActive(slot2, activeSlot[1]);
        RefreshActive(slot3, activeSlot[2]);
        for (int i = 0; i < shopList.items.Count; i++)
        {
            ItemForSale shopListItem = shopList.items[i];
            shopSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = shopListItem.product.Icon;
            shopSlots[i].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = shopListItem.product.ItemName;
            if (shopListItem.bought)
            {
                shopSlots[i].transform.GetChild(2).GetComponent<Image>().enabled = false;
                shopSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Sold!";
            }
            else
            {
                shopSlots[i].transform.GetChild(2).GetComponent<Image>().enabled = true;
                shopSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shopListItem.price + "x";
            }
            shopSlots[i].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            shopSlots[i].transform.GetComponent<Button>().onClick.AddListener(delegate { SetFocusedItem(shopListItem); });
            shopSlots[i].SetActive(true);
        }
    }

    void SetFocusedItem(ItemForSale item)
    {
        focusedItem = item;
        BringUpItem(focusedItem);
    }

    public void DontBuy()
    {
        buyConfirm.DOMove(buyConfirm.position - new Vector3(0, 2500), 0.5f).OnComplete(() => {
            leaving = false;
        });
    }
    
    void AddToActive(int index)
    {
        if (picked)
        {
            return;
        }
        picked = true;
        GameVariables.Instance.AddItemToSlot(index, focusedItem.product);
        print(GameVariables.Instance.GetActiveSlots()[1]);
        AudioManager.PlaySound(activeSelected, 1, 1);
        leaving = true;
        Refresh();
        Invoke("Delay", 1.5f);
    }

    void Delay()
    {
        leaving = false;
        fadeSlots.gameObject.SetActive(false);
        picked = false;
    }

    void BringUpItem(ItemForSale toBringUp)
    {
        if (focusedItem.bought || leaving)
        {
            return;
        }
        foreach (GameObject t in positives)
        {
            t.SetActive(false);
        }
        AudioManager.PlaySound(bringUp, 1, 1.3f);
        confirmTitle.text = toBringUp.product.ItemName;
        confirmDescription.text = toBringUp.product.Description;
        confirmIcon.sprite = toBringUp.product.Icon;
        confirmCost.text = toBringUp.price + "x";
        for (int i = 0; i < toBringUp.product.Positives.Length; i++)
        {
            positives[i].SetActive(true);
            positives[i].GetComponent<TextMeshProUGUI>().text = toBringUp.product.Positives[i];
        }
        buyConfirm.DOMove(confirmPos, 1).SetEase(Ease.Linear);
    }

    IEnumerator LoseCoins(int amount)
    {
        float pitch = 1;
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            AudioManager.PlaySound(decreaseCoinSound, 1, pitch);
            pitch += 0.1f;
            GameVariables.Instance.CoinCollected(-1);
            coinsAmount.text = GameVariables.Instance.GetCoins() + "x";
        }
    }

    public void PurchaseItem()
    {
        if (GameVariables.Instance.GetCoins() < focusedItem.price || focusedItem.bought || leaving)
        {
            return;
        }
        AudioManager.PlaySound(purchaseSound, 1, 1);
        leaving = true;
        focusedItem.bought = true;
        if (!focusedItem.product.Active)
        {
            GameVariables.Instance.AddItemToInventory(focusedItem.product);
        }
        else
        {
            fadeSlots.gameObject.SetActive(true);
        }
        StartCoroutine(LoseCoins(focusedItem.price));
        DontBuy();
        GameVariables.Instance.ReplaceShop(shopList);
        Refresh();
    }

    public void LeaveShop()
    {
        if (leaving)
        {
            return;
        }
        leaving = true;
        Leaving?.Invoke();
        AudioManager.PlaySound(GameVariables.Instance.ButtonClickSound, 0.5f, 0.8f);
        AudioManager.SimpleFadeOut();
        trans.DOMoveY(1100, 2);
        Invoke("ExitShop", 3);
    }

    void ExitShop()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        SceneManager.UnloadSceneAsync(2);
        FindObjectOfType<SelectionStage>().LevelCleared();
    }

    void StartMoving()
    {
        print("start");
        titleText.DOMove(titlePos, 2);
        coin.DOMove(coinPos, 2);
        backButton.DOMoveX(backButton.position.x - 1500, 2);
        coinsAmount.transform.DOMove(coinPos2, 2);
        shopTab.DOMove(shopTabPos, 2);
    }
}
