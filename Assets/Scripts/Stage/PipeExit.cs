using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeExit : MonoBehaviour
{
    public event Action WinState;
    private bool hasWon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || hasWon)
        {
           return;
        }
        GameObject bird = collision.gameObject;
        bird.GetComponent<BirdMovement>().enabled = false;
        bird.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        bird.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        StartCoroutine(MoveIn(bird.transform));
        
        hasWon = true;
        WinState?.Invoke();
        print("here");
    }

    IEnumerator MoveIn(Transform trans)
    {
        for (int i = 0; i < 5; i++)
        {
            trans.DOMove(trans.position + new Vector3(0.2f, 0, 0), 0.15f);
            yield return new WaitForSeconds(0.3f);
        }
        SceneManager.LoadScene(0);
        print("moved");
    }
}
