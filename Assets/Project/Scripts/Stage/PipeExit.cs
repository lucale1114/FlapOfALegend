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
    private ParticleSystem brickbreak;
    private BirdHealth health;
    [SerializeField]
    private AudioClip wallBreakSound;
    [SerializeField]
    private AudioClip pipeEnterNoise;
    private bool cleaner;

    private void Awake()
    {
        brickbreak = transform.parent.Find("Brickbreak").GetComponent<ParticleSystem>();
        health = FindObjectOfType<BirdHealth>();
        cleaner = GameVariables.Instance.ConfirmItemExists("Exit Pipe Cleaner");
    }

    GameObject VictoryTouch(Collider2D collision)
    {
        if (cleaner)
        {
            FindObjectOfType<BirdHealth>().AddHealth(1);
        }
        GameObject bird = collision.gameObject;
        bird.GetComponent<BirdMovement>().enabled = false;
        bird.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        bird.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        hasWon = true;
        WinState?.Invoke();
        return bird;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || hasWon)
        {
            return;
        }
        GameObject bird = VictoryTouch(collision.collider);
        LaunchIn(bird.transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || hasWon)
        {
           return;
        }
        GameObject bird = VictoryTouch(collision);
        StartCoroutine(MoveIn(bird.transform));
    }

    IEnumerator MoveIn(Transform trans)
    {
        for (int i = 0; i < 3; i++)
        {
            trans.DOMove(new Vector3(trans.position.x + 0.2f, 0, 0), 0.15f);
            AudioManager.PlaySound(pipeEnterNoise, 1, 1);
            yield return new WaitForSeconds(0.3f);
        }
        Invoke("WinStage", 2f);
    }

    void WinStage()
    {
        if (health.Health == 0)
        {
            return;
        }
        GameVariables.Instance.SetHealth(FindObjectOfType<BirdHealth>().Health);
        GameVariables.Instance.SetContainers(FindObjectOfType<BirdHealth>().Containers);
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
            SceneManager.UnloadSceneAsync(2);
            FindObjectOfType<SelectionStage>().LevelCleared();
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    void LaunchIn(Transform trans)
    {
        FindObjectOfType<BirdHealth>().TakeDamage();
        trans.DOMove(trans.position + new Vector3(3f, 0, 0), 1f);
        AudioManager.PlaySound(wallBreakSound, 2, 1);
        StartCoroutine(ScreenShake.ShakeScreen(3f, 1.5f));
        var bPos = brickbreak.transform.position;
        bPos.y = trans.position.y;
        brickbreak.transform.position = bPos;
        StartCoroutine(Emit());
        Invoke("WinStage", 2.5f);
    }

    IEnumerator Emit()
    {
        for (int i = 0; i < 5; i++)
        {
            brickbreak.Emit(UnityEngine.Random.Range(3, 8));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
