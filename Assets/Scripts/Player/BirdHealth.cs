using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdHealth : MonoBehaviour
{
    public event Action TakeHit;
    public event Action Died;

    public int Health;
    public int Containers;

    private bool iFrame;

    IEnumerator RemoveIFrame()
    {
        SpriteRenderer sprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        transform.GetChild(0).DOFlip();
        for (int i = 0; i < 4; i++)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.2f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
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
        return true;
    }

    public bool AddHeartContainer(int health)
    {
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
        TakeHit?.Invoke();
        StartCoroutine(ScreenShake.ShakeScreen(1, 1.1f));

        if (Health == 0)
        {
            Died?.Invoke();
            StartCoroutine(DiedFunction());
            return;
        }
        StartCoroutine(RemoveIFrame());
        iFrame = true;
    }
}
