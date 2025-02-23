using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class BirdHealth : MonoBehaviour
{
    public event Action TakeHit;
    public event Action HealthChanged;
    public event Action Died;

    public int Health;
    public int Containers;

    private bool iFrame;
    [SerializeField]
    private AudioClip healSound;
    [SerializeField]
    private AudioClip containerSound;

    IEnumerator FlashSprite(SpriteRenderer sprite)
    {
        float startFade = 0.6f;
        for (int i = 0; i < 4; i++)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.2f);
            startFade += 0.1f;
            sprite.color = new Color(1, 1, 1, startFade);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Awake()
    {
        Health = GameVariables.Instance.GetHealth();
        Containers = GameVariables.Instance.GetContainers();
    }

    IEnumerator RemoveIFrame()
    {
        foreach (Transform item in transform.GetChild(0))
        {
            SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();
            StartCoroutine(FlashSprite(renderer));
        }
        yield return new WaitForSeconds(1.7f);
        iFrame = false;
        //GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator DiedFunction()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, 3).SetUpdate(true);
    }

    public bool AddHealth(int health)
    {
        if (Health == Containers)
        {
            return false;
        }
        AudioManager.PlaySound(healSound, 0.7f, 1);
        Health++;
        HealthChanged?.Invoke();
        return true;
    }

    public bool AddHeartContainer(int health)
    {
        if (Containers == 12)
        {
            return false;
        }
        AudioManager.PlaySound(containerSound, 1, 1);
        Containers++;
        HealthChanged?.Invoke();
        return true;
    }

    public bool CanBeHit()
    {
        if (Health == 0 || iFrame) {
            return false;
        }
        return true;
    }

    public void TakeDamage()
    {
        if (iFrame && Health > 0)
        {
            return;
        }

        //GetComponent<Collider2D>().enabled = false;
        Health--;
        HealthChanged?.Invoke();
        TakeHit?.Invoke();
        StartCoroutine(ScreenShake.ShakeScreen(1, 1.1f));

        if (Health == 0)
        {
            Died?.Invoke();
            StartCoroutine(DiedFunction());
            GameInterface.FlashImage(0.8f, 1f);
            AudioManager.StopMusic();
            return;
        }

        GameInterface.FlashImage(0.8f, 0.2f);
        StartCoroutine(RemoveIFrame());
        iFrame = true;
    }
}
