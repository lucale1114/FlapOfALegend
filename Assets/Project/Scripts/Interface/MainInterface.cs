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
    
    private Blinking blinking;

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
        bird = GameObject.Find("BirdObject").transform;
        cameraParent = Camera.main.transform.parent;
        blinking = FindObjectOfType<Blinking>();
        cosmetics = GameObject.Find("Cosmetics").transform;
    }

    private void Start()
    {
        FindObjectOfType<BirdMovement>().EyesOpen += () =>
        {
            OtherButtonsAway(true);
        };
        pauseButton.position -= new Vector3(1000, 0, 0);
        buttonBack.position -= new Vector3(1000, 0, 0);
        cosmetics.position -= new Vector3(0, 2000, 0);
        loginFieldPosBack = loginField.position + new Vector3(-2000, 0);
        loginFieldPos = loginField.position;

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
    }

    public void GoCosmetic()
    {
        InMenu = true;
        OtherButtonsAway(false);
        Camera.main.transform.parent = Camera.main.transform.parent.parent;
        Camera.main.transform.DOMoveX(bird.transform.position.x, 1f);
        Camera.main.transform.DOMoveY(bird.transform.position.y, 1f);
        Camera.main.DOOrthoSize(2.5f, 0.5f);
        cosmetics.DOMoveY(cosmetics.position.y + 2000, 1f);
        StartCoroutine(blinking.WakeUp(true));
    }

    public void ButtonsBack()
    {
        InMenu = false;
        Camera.main.transform.parent = cameraParent;
        buttonBack.position -= new Vector3(1000, 0, 0);
        Camera.main.DOOrthoSize(5, 1f);
        Camera.main.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f);
        blinking.GoBackToSleep();
        buttonCosmetic.DOMoveX(buttonCosmetic.position.x + 1000, 1f);
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
