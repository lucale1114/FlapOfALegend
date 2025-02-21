using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActiveItems : MonoBehaviour
{
    private Transform inventoryTab;
    private Image openCloseButton;
    private Vector3 downPos;
    private Vector3 upPos;
    private Transform[] slots;
    private GrossActiveItemScript grossScript;
    private Transform badgesSlots;
    private SelectionStage stageScript;
    private Transform inventoryInfo;
    private TextMeshProUGUI invDesc;
    private TextMeshProUGUI invTitle;
    
    private Image invIcon;
    private List<TextMeshProUGUI> proList = new List<TextMeshProUGUI>();
    private Button invClose;
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
    [SerializeField]
    private AudioClip appear;
    private List<FlappyItem> referencedList;

    public FlappyItem test;
    public FlappyItem testActiveItem;

    private void Awake()
    {
        Transform slot1 = transform.Find("Slot1");
        Transform slot2 = transform.Find("Slot2");
        Transform slot3 = transform.Find("Slot3");

        grossScript = GetComponent<GrossActiveItemScript>();
        if (!isInSelection)
        {
            grossScript.enabled = true;
        }
        slots = new Transform[3];
        slots[0] = slot1;
        slots[1] = slot2;
        slots[2] = slot3;

        openCloseButton = transform.Find("OpenActive").GetComponent<Image>();
        if (isInSelection)
        {
            inventoryTab = transform.Find("InventoryTab");
            badgesSlots = inventoryTab.Find("Items");
            stageScript = FindObjectOfType<SelectionStage>();
            stageScript.ClearedLevel += () =>
            {
                transform.position = downPos;
                UpdateBadges();
                UpdateActives(); 
            };
            stageScript.StartingNew += () =>
            {
                transform.DOMove(transform.position - new Vector3(0, 1500), 0.5f);
            };
            inventoryInfo = transform.Find("InventoryInfo");
            inventoryInfo.localScale = new Vector3(0, 0, 0);
            invDesc = inventoryInfo.Find("LabelDesc").GetComponent<TextMeshProUGUI>();
            invTitle = inventoryInfo.Find("LabelName").GetComponent<TextMeshProUGUI>();
            invIcon = inventoryInfo.Find("Icon").GetChild(0).GetComponent<Image>();
            foreach (Transform item in inventoryInfo.Find("Limiter").Find("Positives"))
            {
                proList.Add(item.GetComponent<TextMeshProUGUI>());
            }
        }
    }

    void Start()
    {
        if (GameVariables.Instance.GetSpecialLevel() == "Shop")
        {
            return;
        }
       
        upPos = transform.position;
        if (isInSelection)
        {
            UpdateBadges();
            downPos = transform.position - new Vector3(0, 1450);
        }
        else
        {
            downPos = transform.position - new Vector3(0, 420);
        }
        transform.position = downPos;
        UpdateActives();
    }

    void UpdateBadges()
    {
        GameVariables.Instance.AddItemToSlot(0, testActiveItem);
        GameVariables.Instance.AddItemToInventory(test);
        referencedList = GameVariables.Instance.GetInventory();
        foreach (Transform item in badgesSlots) {
            item.GetComponent<Image>().sprite = emptyBadge;
        }
        for (int i = 0; i < referencedList.Count; i++) {
            badgesSlots.GetChild(i).GetComponent<Image>().sprite = referencedList[i].Icon;
            Button b = badgesSlots.GetChild(i).GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            FlappyItem itemToReference = referencedList[i];
            b.onClick.AddListener(delegate { OpenInfo(itemToReference); } );
        }
    }

    public void CloseInfo()
    {
        AudioManager.PlaySound(appear, 1, -1.3f);
        inventoryInfo.DOScale(new Vector3(0, 0, 0), 0.45f);
    }


    private void OpenInfo(FlappyItem item)
    {
        print(item);
        AudioManager.PlaySound(appear, 1, 1.3f);
        invDesc.text = item.Description;
        invTitle.text = item.ItemName;
        invIcon.sprite = item.Icon;
        for (int i = 0; i < item.Positives.Length; i++)
        {
            proList[i].text = "+ " + item.Positives[i];
        }
        inventoryInfo.DOScale(new Vector3(1, 1, 1), 0.45f);
    }

    private void ClickedSlot(int index, FlappyItem active)
    {
        if (active == null)
        {
            return;
        }
        if (isInSelection)
        {
            OpenInfo(active);
        }
        grossScript.UseItem(active.ItemName);
        GameVariables.Instance.ActiveItemUsed(index);
        UpdateActives();
    }

    public void UpdateActives()
    {
        FlappyItem[] items = GameVariables.Instance.GetActiveSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (items[i] != null)
            {
                Image image = slots[i].GetChild(0).GetComponent<Image>();
                image.sprite = items[i].Icon;
                image.color = new Color(1, 1, 1, 1);
                slots[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].ItemName;
            } else
            {
                Image image = slots[i].GetChild(0).GetComponent<Image>();
                image.color = new Color(1, 1, 1, 0);
                slots[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        slots[0].GetComponent<Button>().onClick.RemoveAllListeners();
        slots[1].GetComponent<Button>().onClick.RemoveAllListeners();
        slots[2].GetComponent<Button>().onClick.RemoveAllListeners();
        slots[0].GetComponent<Button>().onClick.AddListener(delegate { ClickedSlot(0, GameVariables.Instance.GetActiveSlots()[0]); });
        slots[1].GetComponent<Button>().onClick.AddListener(delegate { ClickedSlot(1, GameVariables.Instance.GetActiveSlots()[1]); });
        slots[2].GetComponent<Button>().onClick.AddListener(delegate { ClickedSlot(2, GameVariables.Instance.GetActiveSlots()[2]); });
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
