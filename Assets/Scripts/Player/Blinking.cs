using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    private Animator blinkAnimator;
    private BirdHealth birdHealth;
    private SpriteRenderer render;
    private bool hurt;

    public Sprite BlinkSprite;

    void Awake()
    {
        birdHealth = FindObjectOfType<BirdHealth>();
        blinkAnimator = GetComponent<Animator>();
        InvokeRepeating("BlinkAuto", 1, 3);
        render = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        birdHealth.TakeHit += () =>
        {
            hurt = true;
            blinkAnimator.enabled = false;
            render.sprite = BlinkSprite;
            Invoke("UnHurt", 0.45f);
        };
    }

    void BlinkAuto()
    {
        if (!hurt)
        {
            blinkAnimator.Play("Blink1", 0, 0);
        }
    }

    void UnHurt()
    {
        blinkAnimator.enabled = true;
        hurt = false;
    }

}
