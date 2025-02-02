using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionBird : MonoBehaviour
{
    Transform bird;
    Animator blinkAnimator;
    Animator wingAnimator;

    private void Awake()
    {
        bird = transform;
        blinkAnimator = bird.Find("Body").GetComponent<Animator>();
        wingAnimator = bird.Find("Wings").GetComponent<Animator>();
    }

    private void Start()
    {
        //transform.localPosition -= new Vector3(0.12f, 0);
        FloatUp();
        InvokeRepeating("Blink", 2, 3);
        InvokeRepeating("Wing", 1, 1);
    }

    private void FloatUp()
    {
        transform.DOLocalMoveY(transform.localPosition.y + 0.25f, 1f).OnComplete(() => { FloatDown(); }).SetEase(Ease.OutSine);
    }

    private void FloatDown()
    {
        transform.DOLocalMoveY(transform.localPosition.y - 0.25f, 1f).OnComplete(() => { FloatUp(); }).SetEase(Ease.InSine);
    }

    private void Blink()
    {
        blinkAnimator.Play("Blink1", 0, 0);
    }

    private void Wing()
    {
        wingAnimator.Play("Wings1", 0, 0);
    }
}
