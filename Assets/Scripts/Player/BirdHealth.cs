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
    public Image[] HeartSprites;
    public Sprite FullHeart;
    public Sprite EmptyHeart;

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

    private void Start()
    {
        DisplayHearts();
    }

    private void DisplayHearts()
    {
        for (int i = 0; i < HeartSprites.Length; i++)
        {
            if (i < Health)
            {
                HeartSprites[i].sprite = FullHeart;
            }
            else
            {
                HeartSprites[i].sprite = EmptyHeart;
            }

            if (i < Containers)
            {
                HeartSprites[i].enabled = true;
            }
            else
            {
                HeartSprites[i].enabled = false;
            }
        }
    }

    public bool AddHealth(int health) {
        return true;
    }

    public bool AddHeartContainer(int health)
    {
        return true;
    }

    public bool CanBeHit()
    {
        if (Health == 0 && iFrame) {
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

        TakeHit?.Invoke();
        //GetComponent<Collider2D>().enabled = false;
        Health--;
        DisplayHearts();

        if (Health == 0)
        {
            Died?.Invoke();
            return;
        }
        StartCoroutine(RemoveIFrame());
        iFrame = true;
    }
}
