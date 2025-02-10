using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItems : MonoBehaviour
{
    private Transform inventoryTab;
    private Image openCloseButton;
    private Vector3 downPos;
    private Vector3 upPos;
    private Transform slot1;
    private Transform slot2;
    private Transform slot3;
    private Transform badgesSlots;

    [SerializeField]
    private bool isInSelection;
    [SerializeField]
    private Sprite openButton;
    [SerializeField]
    private Sprite closeButton;
    [SerializeField]
    private Sprite emptyBadge;
    [SerializeField]
    private AudioClip buttonSound;

    public FlappyItem test;

    private void Awake()
    {
        inventoryTab = transform.Find("InventoryTab");
        slot1 = transform.Find("Slot1");
        slot2 = transform.Find("Slot2");
        slot3 = transform.Find("Slot3");
        openCloseButton = transform.Find("OpenActive").GetComponent<Image>();
        badgesSlots = inventoryTab.Find("Items");
    }

    void UpdateBadges()
    {
        List<FlappyItem> inventory = GameVariables.Instance.GetInventory();
        foreach (Transform item in badgesSlots) {
            item.GetComponent<Image>().sprite = emptyBadge;
        }
        for (int i = 0; i < inventory.Count; i++) {
            badgesSlots.GetChild(i).GetComponent<Image>().sprite = inventory[i].Icon;
        }
    }

    void Start()
    {
        GameVariables.Instance.AddItemToInventory(test);
        GameVariables.Instance.AddItemToInventory(test);
        upPos = transform.position;
        if (isInSelection)
        {
            downPos = transform.position - new Vector3(0, 1450);
        }
        else {
            downPos = transform.position - new Vector3(0, 100);
        }
        transform.position = downPos;
        UpdateBadges();
    }

    public void OpenButton()
    {
        if (transform.position == upPos)
        {
            AudioManager.PlaySound(buttonSound, 1, -1.5f);
            openCloseButton.sprite = closeButton;
            MenuDown();
        } 
        else
        {
            AudioManager.PlaySound(buttonSound, 1, 1.5f);
            openCloseButton.sprite = openButton;
            MenuUp();
        }
    }
    void MenuDown() {
        transform.DOMove(downPos, 1).SetEase(Ease.OutBounce);
    }

    void MenuUp()
    {
        transform.DOMove(upPos, 1).SetEase(Ease.OutBounce);
    }
}
