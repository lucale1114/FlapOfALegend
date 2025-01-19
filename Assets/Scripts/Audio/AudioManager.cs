using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    private AudioSource source;
    private BirdMovement bm;

    public AudioClip LevelMusic;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        bm = FindObjectOfType<BirdMovement>();
    }

    void Start()
    {
        bm.WokeUp += () => {
            print("aro");
            StartCoroutine(FadeOutAndPlayNew());
        };
    }

    public IEnumerator FadeOutAndPlayNew()
    {
        source.DOFade(0, 1);
        yield return new WaitForSeconds(1.5f);
        source.clip = LevelMusic;
        source.Play();
        source.DOFade(1, 1);
    }
}
