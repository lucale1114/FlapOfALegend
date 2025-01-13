using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHealth : MonoBehaviour
{
    public event Action TakeHit;
    private bool iFrame;

    IEnumerator RemoveIFrame()
    {
        SpriteRenderer sprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();

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

    public void TakeDamage()
    {
        if (iFrame)
        {
            return;
        }

        TakeHit?.Invoke();
        iFrame = true;
        //GetComponent<Collider2D>().enabled = false;

        StartCoroutine(RemoveIFrame());
    }
}
