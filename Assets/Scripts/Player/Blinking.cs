using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blinking : MonoBehaviour
{
    private Animator blinkAnimator;
    private BirdHealth birdHealth;
    private SpriteRenderer render;
    private bool hurt;
    private bool sleeping;

    public Sprite BlinkSprite;
    public Sprite SleepSprite;

    void Awake()
    {
        birdHealth = FindObjectOfType<BirdHealth>();
        blinkAnimator = GetComponent<Animator>();
        InvokeRepeating("BlinkAuto", 1, 3);
        render = GetComponent<SpriteRenderer>();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            blinkAnimator.enabled = false;
            sleeping = true;
            render.sprite = SleepSprite;
        }
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
        if (!hurt && !sleeping)
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
