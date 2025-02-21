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
    private ParticleSystem sleepParticle;
    private SpriteRenderer eyes;
    private bool dead;
    private float emissionRate;

    public bool Sleeping;
    public Sprite BlinkSprite;
    public Sprite OpenEyesSprite;
    public Sprite SleepSprite;
    public Sprite DeadEyeSprite;
    public bool CanStart;

    void Awake()
    {
        birdHealth = FindObjectOfType<BirdHealth>();
        blinkAnimator = GetComponent<Animator>();
        InvokeRepeating("BlinkAuto", 1, 3);
        eyes = GameObject.Find("Eyes").GetComponent<SpriteRenderer>();
        render = GetComponent<SpriteRenderer>();
        if (Sleeping)
        {
            sleepParticle = GameObject.Find("SleepParticles").GetComponent<ParticleSystem>();
        }
    }

    private void Start()
    {
        birdHealth.TakeHit += () => TakeAHit();
        birdHealth.Died += () =>
        {
            dead = true;
            blinkAnimator.enabled = false;
            render.sprite = OpenEyesSprite;
            eyes.sprite = DeadEyeSprite;
        };
        emissionRate = sleepParticle.emissionRate;
        if (Sleeping)
        {
            blinkAnimator.enabled = false;
            render.sprite = SleepSprite;
        }
    }

    private void TakeAHit()
    {
        if (dead)
        {
            return;
        }
        hurt = true;
        blinkAnimator.enabled = false;
        render.sprite = BlinkSprite;
        Invoke("UnHurt", 0.45f);
    }

    public IEnumerator WakeUp(bool cosmetic)
    {
        sleepParticle.emissionRate = 0;
        for (int i = 0; i < 3; i++)
        {
            render.sprite = OpenEyesSprite;
            yield return new WaitForSeconds(0.1f);
            render.sprite = SleepSprite;
            yield return new WaitForSeconds(0.1f);
        }
        render.sprite = OpenEyesSprite;
        blinkAnimator.enabled = true;
        if (!cosmetic)
        {
            yield return new WaitForSeconds(0.8f);
            CanStart = true;
        }
    }

    public void GoBackToSleep()
    {
        Sleeping = true;
        sleepParticle.emissionRate = emissionRate;
        blinkAnimator.enabled = false;
        render.sprite = SleepSprite;
        CanStart = false;
    }
    
    void BlinkAuto()
    {
        if (!hurt && !Sleeping)
        {
            blinkAnimator.Play("Blink1", 0, 0);
        }
    }

    void UnHurt()
    {
        if (!dead)
        {
            blinkAnimator.enabled = true;
            hurt = false;
        }
    }

}
