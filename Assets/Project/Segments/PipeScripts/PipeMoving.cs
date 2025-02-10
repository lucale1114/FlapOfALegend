using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PipeMoving : MonoBehaviour
{
    [SerializeField]
    private bool moveUp;
    private Vector3 posUp;
    private Vector3 posDown;
    [SerializeField]
    private float distance = 0.5f;
    [SerializeField]
    private bool random;
    [SerializeField]
    private float slowSpeed = 1;

    void Start()
    {
        if (random)
        {
            if (Random.Range(0,1) == 0)
            {
                moveUp = true;
            }
        }
        posUp = transform.position + new Vector3(0, distance, 0);
        posDown = transform.position - new Vector3(0, distance, 0);
        if (moveUp)
        {
            transform.position = posDown;
            MoveTo(posDown);
        }
        else
        {
            transform.position = posUp;
            MoveTo(posUp);
        }
    }

    void MoveTo(Vector3 pos)
    {
        transform.DOMove(pos, slowSpeed).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (pos == posUp)
            {
                MoveTo(posDown);
            }
            else
            {
                MoveTo(posUp);
            }
        });
    }
}