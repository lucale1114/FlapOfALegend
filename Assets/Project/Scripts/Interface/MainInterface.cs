using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainInterface : MonoBehaviour
{
    private TextMeshProUGUI winCounter;
    private Transform healthBar;
    private Transform pauseButton;
    [SerializeField]
    private Transform loginField;
    [SerializeField]
    private Transform registerField;
    [SerializeField]
    private Transform trans;
    private Transform bird;
    private Transform cameraParent;
    private Vector3 loginFieldPos;
    private Vector3 loginFieldPosBack;

    private FirebaseLogin fbLogin;
    private Transform buttonLogin;
    private Transform buttonRegister;
    private Transform buttonSign;
    private Transform buttonCosmetic;
    private Transform buttonBack;
    private Transform cosmetics;
    private Transform selectionSlot;

    private Transform wingSlot;
    private Transform beakSlot;
    private Transform tapToPlay;
    private Transform eyesSlot;
    private Transform bodySlot;
    private Transform hatSlot;
    private Vector3 bringSlotPos;
    private Blinking blinking;
    private Transform cSlot;
    private Cosmetics cosmeticsScript;
    private AudioClip buttonClick;
    private TextMeshProUGUI equippedText;
    [SerializeField]
    private AudioClip equipSound;

    private bool canPress = true;

    public bool InMenu;

    private void Awake()
    {
        winCounter = GameObject.Find("WinCounter").GetComponent<TextMeshProUGUI>();
        healthBar = GameObject.Find("Lives").transform;
        pauseButton = GameObject.Find("PauseButton").transform;
        fbLogin = FindObjectOfType<FirebaseLogin>();
        buttonLogin = GameObject.Find("ButtonLogin").transform;
        buttonRegister = GameObject.Find("ButtonRegister").transform;
        buttonSign = GameObject.Find("ButtonSignOut").transform;
        buttonCosmetic = GameObject.Find("ButtonCustomize").transform;
        buttonBack = GameObject.Find("ButtonBack").transform;
        cosmeticsScript = FindObjectOfType<Cosmetics>();
        bird = GameObject.Find("BirdObject").transform;
        cameraParent = Camera.main.transform.parent;
        blinking = FindObjectOfType<Blinking>();
        cosmetics = GameObject.Find("Cosmetics").transform;
        wingSlot = GameObject.Find("SlotWings").transform;
        bodySlot = GameObject.Find("SlotBody").transform;
        eyesSlot = GameObject.Find("SlotEyes").transform;
        hatSlot = GameObject.Find("SlotHat").transform;
        beakSlot = GameObject.Find("SlotBeak").transform;
        selectionSlot = GameObject.Find("ItemSlots").transform;
        cSlot = GameObject.Find("CSlot").transform;
        bringSlotPos = selectionSlot.transform.position;
        buttonClick = GameVariables.Instance.ButtonClickSound;
        equippedText = GameObject.Find("ItemEquippedName").GetComponent<TextMeshProUGUI>();
        tapToPlay = GameObject.Find("TapToStart").transform;
    }

    private void CanPress()
    {
        canPress = true;
    }
    private void Start()
    {
        FindObjectOfType<BirdMovement>().EyesOpen += () =>
        {
            tapToPlay.DOMoveX(tapToPlay.position.x - 1200, 2).SetEase(Ease.OutFlash);
            tapToPlay.DOLocalRotate(new Vector3(0, 0, 720), 1f, RotateMode.FastBeyond360).SetRelative(true);
            OtherButtonsAway(true);
        };
        pauseButton.position -= new Vector3(1000, 0, 0);
        buttonBack.position -= new Vector3(1000, 0, 0);
        selectionSlot.position -= new Vector3(2000, 0, 0);
        cosmetics.position -= new Vector3(0, 2000, 0);
        loginFieldPosBack = loginField.position + new Vector3(-2000, 0);
        loginFieldPos = loginField.position;
        cSlot.gameObject.SetActive(false);

        loginField.position = loginFieldPosBack;
        registerField.position += loginFieldPosBack;

        FindObjectOfType<BirdMovement>().WokeUp += () =>
        {
            winCounter.transform.DOMove(winCounter.transform.position + new Vector3(0, 500, 0), 2);
            buttonLogin.DOMove(buttonLogin.position + new Vector3(0, 500, 0), 2);
            buttonRegister.DOMove(buttonRegister.position + new Vector3(0, 500, 0), 2);
            buttonSign.DOMove(buttonSign.position + new Vector3(0, 500, 0), 2);
            healthBar.GetComponent<CanvasGroup>().DOFade(1, 2);
            pauseButton.DOMoveX(pauseButton.position.x + 1000, 2);
        };

        fbLogin.LoggedIn += () => {
            Login();
        };
        fbLogin.Registered += () => {
            Login();
        };
        buttonLogin.GetComponent<Button>().onClick.AddListener(delegate { BringField(loginField, false); });
        buttonRegister.GetComponent<Button>().onClick.AddListener(delegate { BringField(registerField, false); });
        buttonSign.GetComponent<Button>().onClick.AddListener(delegate { buttonLogin.gameObject.SetActive(true); buttonRegister.gameObject.SetActive(true); });
        buttonSign.gameObject.SetActive(false);
        Invoke("DelaySure", 1);
    }

    private void DelaySure()
    {
        LoadItemIcons();
        wingSlot.GetComponent<Button>().onClick.AddListener(delegate { BringUpSlots(GameVariables.Instance.GetWings(), wingSlot); });
        beakSlot.GetComponent<Button>().onClick.AddListener(delegate { BringUpSlots(GameVariables.Instance.GetBeak(), beakSlot); });
        eyesSlot.GetComponent<Button>().onClick.AddListener(delegate { BringUpSlots(GameVariables.Instance.GetEyes(), eyesSlot); });
        hatSlot.GetComponent<Button>().onClick.AddListener(delegate { BringUpSlots((FlappyHat)GameVariables.Instance.GetHat(), hatSlot); });
        bodySlot.GetComponent<Button>().onClick.AddListener(delegate { BringUpSlots(GameVariables.Instance.GetBody(), bodySlot); });
    }

    private void LoadItemIcons()
    {
        wingSlot.GetChild(0).GetComponent<Image>().sprite = GameVariables.Instance.GetWings().iconSprite;
        bodySlot.GetChild(0).GetComponent<Image>().sprite = GameVariables.Instance.GetBody().iconSprite;
        eyesSlot.GetChild(0).GetComponent<Image>().sprite = GameVariables.Instance.GetEyes().iconSprite;
        beakSlot.GetChild(0).GetComponent<Image>().sprite = GameVariables.Instance.GetBeak().iconSprite;

        if (GameVariables.Instance.GetHat() != null)
        {
            hatSlot.GetChild(0).GetComponent<Image>().enabled = true;
            hatSlot.GetChild(0).GetComponent<Image>().sprite = GameVariables.Instance.GetHat().iconSprite;
        }
        else
        {
            hatSlot.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }

    private void BringUpSlots(Object picked, Transform slot)
    {
        AudioManager.PlaySound(buttonClick, 1, 1.5f);
        foreach (Transform i in cSlot.parent)
        {
            if (i.name == "New")
            {
                Destroy(i.gameObject);
            }
        }
        slot.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.6f);
        selectionSlot.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
        selectionSlot.DOMove(bringSlotPos, 0.5f);
        switch (picked)
        {
            case FlappyWings t1:
                foreach (var item in GameVariables.Instance.GetWingList())
                {
                    GameObject newSlot = Instantiate(cSlot.gameObject, cSlot.parent);
                    newSlot.name = "New";
                    newSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.iconSprite;
                    newSlot.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(item); });
                    newSlot.SetActive(true);
                }
                
                break;
            case FlappyBeak t2:
                foreach (var item in GameVariables.Instance.GetBeakList())
                {
                    GameObject newSlot = Instantiate(cSlot.gameObject, cSlot.parent);
                    newSlot.name = "New";
                    newSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.iconSprite;
                    newSlot.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(item); });
                    newSlot.SetActive(true);
                }
                break;
            case FlappyEyes t3:
                foreach (var item in GameVariables.Instance.GetEyesList())
                {
                    GameObject newSlot = Instantiate(cSlot.gameObject, cSlot.parent);
                    newSlot.name = "New";
                    newSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.iconSprite;
                    newSlot.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(item); });
                    newSlot.SetActive(true);
                }
                break;
            case FlappyHat t4:
                print(GameVariables.Instance.GetHatList());
                GameObject empty = Instantiate(cSlot.gameObject, cSlot.parent);
                empty.name = "New";
                empty.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0);
                empty.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(null); });
                empty.SetActive(true);
                foreach (var item in GameVariables.Instance.GetHatList())
                {
                    GameObject newSlot = Instantiate(cSlot.gameObject, cSlot.parent);
                    newSlot.name = "New";
                    newSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.iconSprite;
                    newSlot.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(item); });
                    newSlot.SetActive(true);
                }

                break;
            case FlappyBody t5:
                foreach (var item in GameVariables.Instance.GetBodyList())
                {
                    GameObject newSlot = Instantiate(cSlot.gameObject, cSlot.parent);
                    newSlot.name = "New";
                    newSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.iconSprite;
                    newSlot.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(item); });
                    newSlot.SetActive(true);
                }
                break;
            default:
                foreach (var item in GameVariables.Instance.GetHatList())
                {
                    GameObject newSlot = Instantiate(cSlot.gameObject, cSlot.parent);
                    newSlot.name = "New";
                    newSlot.transform.GetChild(0).GetComponent<Image>().sprite = item.iconSprite;
                    newSlot.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(item); });
                    newSlot.SetActive(true);
                }
                GameObject empty2 = Instantiate(cSlot.gameObject, cSlot.parent);
                empty2.name = "New";
                empty2.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0);
                empty2.GetComponent<Button>().onClick.AddListener(delegate { EquipNewItem(null); });
                empty2.SetActive(true);
                break;
        }
    }
    private bool CompareItems(Object t, Object t2)
    {
        if (t == t2)
        {
            return true;
        }
        return false;
    }

    private void EquipNewItem(Object newCos)
    {
        
        AudioManager.PlaySound(buttonClick, 1, 1.5f);
        if (newCos == null)
        {
            GameVariables.Instance.SetHat(null);
            equippedText.text = "None";
            FinishEquip();
            return;
        }
        switch (newCos)
        {
            case FlappyWings t1:
                if (CompareItems(newCos, GameVariables.Instance.GetWings()))
                {
                    return;
                }
                GameVariables.Instance.SetWings((FlappyWings)newCos);
                equippedText.text = GameVariables.Instance.GetWings().itemName;
                break;
            case FlappyBeak t1:
                if (CompareItems(newCos, GameVariables.Instance.GetBeak()))
                {
                    return;
                }
                GameVariables.Instance.SetBeak((FlappyBeak)newCos);
                equippedText.text = GameVariables.Instance.GetBeak().itemName;
                break;
            case FlappyEyes t1:
                if (CompareItems(newCos, GameVariables.Instance.GetEyes()))
                {
                    return;
                }
                GameVariables.Instance.SetEyes((FlappyEyes)newCos);
                equippedText.text = GameVariables.Instance.GetEyes().itemName;
                break;
            case FlappyBody t1:
                if (CompareItems(newCos, GameVariables.Instance.GetBody()))
                {
                    return;
                }
                GameVariables.Instance.SetBody((FlappyBody)newCos);
                equippedText.text = GameVariables.Instance.GetBody().itemName;
                break;
            case FlappyHat t1:
                if (CompareItems(newCos, GameVariables.Instance.GetHat()))
                {
                    return;
                }
                GameVariables.Instance.SetHat((FlappyHat)newCos);
                equippedText.text = GameVariables.Instance.GetHat().itemName;
                break;
        }
        FinishEquip();
        
    }
    private void FinishEquip()
    {
        StopCoroutine(StartToFadeText());
        equippedText.DOKill();
        equippedText.DOFade(1, 0);
        StartCoroutine(StartToFadeText());
        AudioManager.PlaySound(equipSound, 1, 1.6f);
        StartCoroutine(ScreenShake.ShakeScreen(1f, 0.2f, Camera.main.transform.position));
        LoadItemIcons();
        cosmeticsScript.DressUpBirdGame();
        cosmeticsScript.DressUpUI();
    }

    IEnumerator StartToFadeText()
    {
        yield return new WaitForSeconds(2);
        equippedText.DOFade(0, 2f);
    }

    public void GoCosmetic()
    {
        if (!canPress)
        {
            return;
        }
        canPress = false;
        Invoke("CanPress", 1.05f);
        InMenu = true;
        OtherButtonsAway(false);
        tapToPlay.GetComponent<Image>().DOFade(0, 0.5f);
        Camera.main.transform.parent = Camera.main.transform.parent.parent;
        Camera.main.transform.DOMoveX(bird.transform.position.x, 1f);
        Camera.main.transform.DOMoveY(bird.transform.position.y, 1f);
        Camera.main.DOOrthoSize(2.5f, 0.5f);
        cosmetics.DOMoveY(cosmetics.position.y + 2000, 1f);
        StartCoroutine(blinking.WakeUp(true));
    }

    public void ButtonsBack()
    {
        if (!canPress)
        {
            return;
        }
        canPress = false;
        Invoke("CanPress", 1.05f);
        InMenu = false;
        Camera.main.transform.parent = cameraParent;
        buttonBack.position -= new Vector3(1000, 0, 0);
        Camera.main.DOOrthoSize(5, 1f);
        Camera.main.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f);
        blinking.GoBackToSleep();
        buttonCosmetic.DOMoveX(buttonCosmetic.position.x + 1000, 1f);
        selectionSlot.DOMove(selectionSlot.position - new Vector3(2000, 0, 0), 0.5f);
        tapToPlay.GetComponent<Image>().DOFade(1, 0.5f);
        equippedText.DOFade(0, 0);
        cosmetics.DOMoveY(cosmetics.position.y - 2000, 1f);
    }

    private void OtherButtonsAway(bool wake)
    {
        Camera.main.transform.parent = cameraParent;
        buttonCosmetic.DOMoveX(buttonCosmetic.position.x - 1000, 1f);
        if (!wake)
        {
           buttonBack.position += new Vector3(1000, 0, 0);
        }  
    }

    private void Login()
    {
        FieldsBack();
        buttonLogin.gameObject.SetActive(false);
        buttonRegister.gameObject.SetActive(false);
        buttonSign.gameObject.SetActive(true);
    }

    public void FieldsBack()
    {
        loginField.DOMove(loginFieldPosBack, 1);
        registerField.DOMove(loginFieldPosBack, 1);
    }

    private void BringField(Transform field, bool back)
    {
        FieldsBack();

        if (!back)
        {
            field.gameObject.SetActive(true);
            field.DOKill();
            field.DOMove(loginFieldPos, 1);
        }
        else
        {
            field.DOMove(loginFieldPosBack, 1).OnComplete(delegate { field.gameObject.SetActive(false); });
        }
    }
}
