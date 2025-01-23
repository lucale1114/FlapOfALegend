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
    private BirdHealth health;
    [SerializeField]
    private Transform loginField;
    [SerializeField]
    private Transform registerField;
    private Vector3 loginFieldPos;
    private Vector3 loginFieldPosBack;

    private FirebaseLogin fbLogin;
    private Transform buttonLogin;
    private Transform buttonRegister;
    private Transform buttonSign;

    public Image[] HeartSprites;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    private void Awake()
    {
        winCounter = GameObject.Find("WinCounter").GetComponent<TextMeshProUGUI>();
        healthBar = GameObject.Find("Lives").transform;
        health = FindObjectOfType<BirdHealth>();
        fbLogin = FindObjectOfType<FirebaseLogin>();
        buttonLogin = GameObject.Find("ButtonLogin").transform;
        buttonRegister = GameObject.Find("ButtonRegister").transform;
        buttonSign = GameObject.Find("ButtonSignOut").transform;
    }

    private void Start()
    {
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
        };
        DisplayHearts();
        health.TakeHit += () =>
        {
            DisplayHearts();
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

    private void DisplayHearts()
    {
        for (int i = 0; i < HeartSprites.Length; i++)
        {
            if (i < health.Health)
            {
                HeartSprites[i].sprite = FullHeart;
            }
            else
            {
                HeartSprites[i].sprite = EmptyHeart;
            }

            if (i < health.Containers)
            {
                HeartSprites[i].enabled = true;
            }
            else
            {
                HeartSprites[i].enabled = false;
            }
        }
    }
}
