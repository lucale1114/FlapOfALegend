using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Cosmetics : MonoBehaviour
{
    [SerializeField]
    public CosmeticsList cosmeticList;
    [SerializeField]
    private bool inSelection;

    private Transform beak;
    private Transform body;
    private Transform eyes;
    private Transform wings;
    private Transform hat;

    private Transform beakUI;
    private Transform bodyUI;
    private Transform eyesUI;
    private Transform wingsUI;
    private Transform hatUI;

    void Awake()
    {
        body = transform.Find("Body");
        beak = transform.Find("Beak");
        eyes = transform.Find("Eyes");
        wings = transform.Find("Wings");
        hat = transform.Find("Hat");
        if (!inSelection)
        {
            bodyUI = GameObject.Find("BodyUI").transform;
            beakUI = GameObject.Find("BeakUI").transform;
            eyesUI = GameObject.Find("EyesUI").transform;
            wingsUI = GameObject.Find("WingsUI").transform;
            hatUI = GameObject.Find("HatUI").transform;
        }
    }

    public void DressUpBirdGame()
    {
        FlappyBody flappyBody = GameVariables.Instance.GetBody();
        FlappyBeak flappyBeak = GameVariables.Instance.GetBeak();
        FlappyEyes flappyEyes = GameVariables.Instance.GetEyes();
        FlappyWings flappyWings = GameVariables.Instance.GetWings();
        FlappyHat flappyHat = GameVariables.Instance.GetHat();

        Blinking blinking = body.GetComponent<Blinking>();
        blinking.DeadEyeSprite = flappyEyes.deadEye;

        if (blinking.Sleeping)
        {
            body.GetComponent<SpriteRenderer>().sprite = flappyBody.sleepSprite;
        }
        else
        {
            body.GetComponent<SpriteRenderer>().sprite = flappyBody.openEyes;
        }
        body.GetComponent<Animator>().keepAnimatorStateOnDisable = true;
        body.GetComponent<Animator>().SetInteger("Blink", flappyBody.itemId);
        blinking.SetAnimationName(flappyBody.animation.name);

        wings.GetComponent<SpriteRenderer>().sprite = flappyWings.iconSprite;
        wings.GetComponent<Animator>().SetInteger("Wing", flappyWings.itemId);
        wings.parent.parent.GetComponent<BirdMovement>().GlideSprite = flappyWings.glide;
        wings.parent.parent.GetComponent<BirdMovement>().SetWingsAnimation(flappyWings.animation.name);

        blinking.OpenEyesSprite = flappyBody.openEyes;
        blinking.BlinkSprite = flappyBody.blinkSprite;
        blinking.SleepSprite = flappyBody.sleepSprite;

        if (flappyHat)
        {
            hat.GetComponent<SpriteRenderer>().DOFade(1, 0);
            hat.GetComponent<SpriteRenderer>().sprite = flappyHat.iconSprite;
            hat.transform.localPosition = flappyHat.localPos;
        }
        else 
        {
            hat.GetComponent<SpriteRenderer>().DOFade(0, 0);
        }

        beak.GetComponent<SpriteRenderer>().sprite = flappyBeak.iconSprite;
        eyes.GetComponent<SpriteRenderer>().sprite = flappyEyes.iconSprite;
    }

    private void DressUpSelection()
    {
        FlappyBody flappyBody = GameVariables.Instance.GetBody();
        FlappyBeak flappyBeak = GameVariables.Instance.GetBeak();
        FlappyEyes flappyEyes = GameVariables.Instance.GetEyes();
        FlappyWings flappyWings = GameVariables.Instance.GetWings();
        FlappyHat flappyHat = GameVariables.Instance.GetHat();

        SelectionBird sb = body.parent.GetComponent<SelectionBird>();

        body.GetComponent<Animator>().keepAnimatorStateOnDisable = true;
        body.GetComponent<Animator>().SetInteger("Blink", flappyBody.itemId);
        print(flappyBody.animation.name);
        sb.SetAnimationNameB(flappyBody.animation.name);

        wings.GetComponent<SpriteRenderer>().sprite = flappyWings.iconSprite;
        wings.GetComponent<Animator>().SetInteger("Wing", flappyWings.itemId);
        sb.SetAnimationNameW(flappyWings.animation.name);

        if (flappyHat)
        {
            hat.GetComponent<SpriteRenderer>().DOFade(1, 0);
            hat.GetComponent<SpriteRenderer>().sprite = flappyHat.iconSprite;
            hat.transform.localPosition = flappyHat.localPos;
        }
        else
        {
            hat.GetComponent<SpriteRenderer>().DOFade(0, 0);
        }
        beak.GetComponent<SpriteRenderer>().sprite = flappyBeak.iconSprite;
        eyes.GetComponent<SpriteRenderer>().sprite = flappyEyes.iconSprite;
    }

    public void DressUpUI()
    {
        FlappyBody flappyBody = GameVariables.Instance.GetBody();
        FlappyBeak flappyBeak = GameVariables.Instance.GetBeak();
        FlappyEyes flappyEyes = GameVariables.Instance.GetEyes();
        FlappyWings flappyWings = GameVariables.Instance.GetWings();
        FlappyHat flappyHat = GameVariables.Instance.GetHat();

        wingsUI.GetComponent<Image>().sprite = flappyWings.iconSprite;
        wingsUI.GetComponent<Animator>().SetInteger("Wing", flappyWings.itemId);
        wingsUI.GetComponent<Animator>().Play(flappyWings.animation.name);

        beakUI.GetComponent<Image>().sprite = flappyBeak.iconSprite;
        bodyUI.GetComponent<Image>().sprite = flappyBody.openEyes;
        eyesUI.GetComponent<Image>().sprite = flappyEyes.deadEye;
        if (flappyHat)
        {
            hatUI.GetComponent<Image>().DOFade(1, 0);
            hatUI.GetComponent<Image>().sprite = flappyHat.iconSprite;
            hatUI.GetComponent<RectTransform>().localPosition = flappyHat.localPosUI;
            hatUI.GetComponent<RectTransform>().sizeDelta = new Vector2(flappyHat.uiWidth, flappyHat.uiHeight);
        }
        else
        {
            hat.GetComponent<Image>().DOFade(0, 0);
        }
    }

    void Start()
    {
        SetToDefault();

        if (inSelection)
        {
            Invoke("DressUpSelection", 0.2f);
        }
        else
        {
            Invoke("DressUpBirdGame", 0.2f);
            Invoke("DressUpUI", 0.2f);
        }
        foreach (var item in cosmeticList.beaks)
        {
            GameVariables.Instance.AddBeak(item);
        }
        foreach (var item in cosmeticList.eyes)
        {
            GameVariables.Instance.AddEyes(item);
        }
        foreach (var item in cosmeticList.bodies)
        {
            GameVariables.Instance.AddBody(item);
        }
        foreach (var item in cosmeticList.hats)
        {
            GameVariables.Instance.AddHat(item);
        }
        foreach (var item in cosmeticList.wings)
        {
            GameVariables.Instance.AddWings(item);
        }
    }

    void SetToDefault()
    {
        if (GameVariables.Instance.GetBody() != null)
        {
            return;
        }
        GameVariables.Instance.SetBeak(cosmeticList.beaks[0]);
        GameVariables.Instance.SetWings(cosmeticList.wings[0]);
        GameVariables.Instance.SetBody(cosmeticList.bodies[1]);
        GameVariables.Instance.SetHat(cosmeticList.hats[0]);
        GameVariables.Instance.SetEyes(cosmeticList.eyes[0]);
    }
}
