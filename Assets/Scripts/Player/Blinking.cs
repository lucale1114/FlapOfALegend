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

    public bool Sleeping;
    public Sprite BlinkSprite;
    public Sprite OpenEyesSprite;
    public Sprite SleepSprite;
    public bool CanStart;

    void Awake()
    {
        birdHealth = FindObjectOfType<BirdHealth>();
        blinkAnimator = GetComponent<Animator>();
        InvokeRepeating("BlinkAuto", 1, 3);
        render = GetComponent<SpriteRenderer>();
        sleepParticle = GameObject.Find("SleepParticles").GetComponent<ParticleSystem>();
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
        if (Sleeping)
        {
            blinkAnimator.enabled = false;
            render.sprite = SleepSprite;
        }
    }

    public IEnumerator WakeUp()
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
        CanStart = true;
        blinkAnimator.enabled = true;
    }

    void FlapAgain()
    {

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
        blinkAnimator.enabled = true;
        hurt = false;
    }

}
