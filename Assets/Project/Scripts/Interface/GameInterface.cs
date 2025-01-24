using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameInterface : MonoBehaviour
{
    public Image[] HeartSprites;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

    [SerializeField]
    private Transform trans;
    private BirdHealth health;
    private PipeExit winCondition;
    private static Image flash;

    private void Awake()
    {
        health = FindObjectOfType<BirdHealth>();
        flash = GameObject.Find("Flash").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DisplayHearts();
        health.TakeHit += () =>
        {
            DisplayHearts();
        };
        Invoke("FindDelay", 1);
    }

    void FindDelay()
    {
        winCondition = FindObjectOfType<PipeExit>();
        winCondition.WinState += () =>
        {
            Invoke("Transition", 2f);
        };
    }

    private void Transition()
    {
        if (health.Health == 0) { return; }
        trans.DOMove(trans.position - new Vector3(1700, 0), 1.2f).SetEase(Ease.Linear);
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

    public static void FlashImage(float colorStart, float fTime)
    {
        flash.color = new Color(1, 1, 1, colorStart);
        flash.DOColor(new Color(1, 1, 1, 0), fTime).SetEase(Ease.Linear).SetUpdate(true);
    }
}
